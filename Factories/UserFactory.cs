using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using PlaceFinder.Models;
 
namespace PlaceFinder.Factory
{
    public class UserFactory : IFactory<User>
    {
        private readonly IOptions<MySqlOptions> MySqlConfig;

        public UserFactory(IOptions<MySqlOptions> config)
        {
            MySqlConfig = config;
        }
        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(MySqlConfig.Value.ConnectionString);
            }
        }

        public User FindById(int userId)
        {
            using (IDbConnection dbConnection = Connection)
            {
                // get User based on userId
                string UserQ = @"
                    SELECT * FROM users WHERE _id = @UserId;
                ";
                // get Places based on join table Users_Places for given user
                string PlaceQ = $@"
                    SELECT p.* FROM users_places up
                    INNER JOIN places p ON p._id = up.places__id
                    WHERE up.users__id = @UserId;
                ";
                // get PlaceTypes, PlaceHours, & PlacePhotos based on Place._id
                string PlaceChildrenQ = @"
                    SELECT * FROM types WHERE places__id = @PlaceId;
                    SELECT * FROM hours WHERE places__id = @PlaceId;
                    SELECT * FROM photos WHERE places__id = @PlaceId;

                    SELECT r.*, u.name 
                    FROM reviews r JOIN users u
                    ON r.users__id = u._id
                    AND r.places__id = @PlaceId;
                ";

                dbConnection.Open();
                var _User = dbConnection.Query<User>(UserQ, new { UserId = userId }).FirstOrDefault();
                var _Places = dbConnection.Query<Place>(PlaceQ, new { UserId = userId }).ToList();

                // iterate over each place and query for types, hours, & photos
                _Places.ForEach(p => {
                    using (var multi = dbConnection.QueryMultiple(PlaceChildrenQ, new { PlaceId = p._id, UserId = userId }))
                    {
                        p.types = multi.Read<PlaceTypes>().ToList();
                        p.hours = multi.Read<PlaceHours>().ToList();
                        p.photos = multi.Read<PlacePhotos>().ToList();
                        p.reviews = multi.Read<Review>().ToList();
                    }
                });

                _User.places = _Places;
                return _User;
            }
        }

        public List<string> GetUsersPlaceIds(int userId)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string PlaceIdQ = @"
                    SELECT p.place_id FROM users_places up
                    INNER JOIN places p ON p._id = up.places__id
                    WHERE users__id = @UserId
                ";
                List<string> PlaceIds = new List<string>();

                dbConnection.Open();
                var _Places = dbConnection.Query<Place>(PlaceIdQ, new { UserId = userId }).ToList();
                foreach (var place in _Places)
                {
                    PlaceIds.Add(place.place_id);
                }
                return PlaceIds;
            }
        }

        // if the user exists return the User
        // if user doesnt exist, create new User and return new User
        public User FindOrCreate(string uName)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string UserQ = @"
                    SELECT * FROM users WHERE name = @Name
                ";
                dbConnection.Open();
                var user = dbConnection.Query<User>(UserQ, new { Name = uName }).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
                else
                {
                    string InsertQ = @"
                        INSERT INTO users (name, created_at, updated_at) 
                        VALUES (@Name, NOW(), NOW())
                    ";                    
                    dbConnection.Execute(InsertQ, new { Name = uName });
                    return dbConnection.Query<User>(UserQ, new { Name = uName }).FirstOrDefault();
                }
            }
        }

        public ICollection<User> GetAllUsers()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string UsersQ = @"
                    SELECT _id, name FROM users;
                ";
                // get Places based on join table Users_Places for given user
                string PlacesCountQ = @"
                    SELECT p._id FROM users_places up
                    INNER JOIN places p ON p._id = up.users__id
                    WHERE users__id = @UserId;
                ";

                var _Users = dbConnection.Query<User>(UsersQ).ToList();
                _Users.ForEach(u => {
                    var _PCount = dbConnection.Query<Place>(PlacesCountQ, new { UserId = u._id }).Count();
                    u.places_count = _PCount;
                });
                return _Users;
            }
        }

        public Review AddReview(int UserId, int PlaceId, string Review)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string InsertQ = @"
                    INSERT INTO reviews (text, users__id, places__id, created_at, updated_at)
                    VALUES (@Text, @UserId, @PlaceId, NOW(), NOW())
                ";
                // query for review and joins user.name onto review
                string ReviewQ = @"
                    SELECT r.*, u.name
                    FROM reviews r JOIN users u
                    ON r.users__id = u._id
                    WHERE r.text = @Text AND r.users__id = @UserId AND r.places__id = @PlaceId
                ";
                dbConnection.Open();
                dbConnection.Execute(InsertQ, new { Text = Review, UserId = UserId, PlaceId = PlaceId });
                return dbConnection.Query<Review>(ReviewQ, new { Text = Review, UserId = UserId, PlaceId = PlaceId }).FirstOrDefault();
            }
        }
    }
}