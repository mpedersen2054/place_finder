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
                // get Places based on userId
                string PlaceQ = $@"
                    SELECT * FROM places WHERE users__id = @UserId;
                ";
                // get PlaceTypes, PlaceHours, & PlacePhotos based on Place._id
                string PlaceChildrenQ = @"
                    SELECT * FROM types WHERE places__id = @PlaceId;
                    SELECT * FROM hours WHERE places__id = @PlaceId;
                    SELECT * FROM photos WHERE places__id = @PlaceId;
                ";

                dbConnection.Open();

                var _User = dbConnection.Query<User>(UserQ, new { UserId = userId }).FirstOrDefault();
                var _Places = dbConnection.Query<Place>(PlaceQ, new { UserId = userId }).ToList();

                // iterate over each place and query for types, hours, & photos
                _Places.ForEach(p => {
                    using (var multi = dbConnection.QueryMultiple(PlaceChildrenQ, new { PlaceId = p._id }))
                    {
                        p.types = multi.Read<PlaceTypes>().ToList();
                        p.hours = multi.Read<PlaceHours>().ToList();
                        p.photos = multi.Read<PlacePhotos>().ToList();
                    }
                });

                _User.places = _Places;
                return _User;
            }
        }

        // if the user exists return the User
        // if user doesnt exist, create new User and return new User
        public User FindOrCreate(string uName)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string QueryName = @"
                    SELECT * FROM users
                    WHERE name = @Name
                ";
                dbConnection.Open();
                var user = dbConnection.Query<User>(QueryName, new { Name = uName }).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
                else
                {
                    string QueryInsert = @"
                        INSERT INTO users (name, created_at, updated_at) 
                        VALUES (@Name, NOW(), NOW())
                    ";                    
                    dbConnection.Execute(QueryInsert, new { Name = uName });
                    return dbConnection.Query<User>(QueryName, new { Name = uName }).FirstOrDefault();
                }
            }
        }
    }
}