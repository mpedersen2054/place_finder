using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using PlaceFinder.Models;
 
namespace PlaceFinder.Factory
{
    public class PlaceFactory : IFactory<User>
    {
        private readonly IOptions<MySqlOptions> MySqlConfig;

        public PlaceFactory(IOptions<MySqlOptions> config)
        {
            MySqlConfig = config;
        }
        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(MySqlConfig.Value.ConnectionString);
            }
        }

        public void CreatePlace(int userId, PlaceResults place)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                string ExistsQ = @"
                    SELECT * FROM places
                    WHERE formatted_address = @PlaceAddr
                ";
                // check if the place already exists
                var PlaceExists = dbConnection.Query<Place>(ExistsQ, new { PlaceAddr = place.formatted_address }).FirstOrDefault();
                if (PlaceExists != null)
                {
                    // -> if it does, check if the user already has a relationship with the place
                    string RelQ = @"
                        SELECT * FROM users_places
                        WHERE users__id = @UserId AND places__id = @PlaceId
                    ";
                    var RelExists = dbConnection.Query<UsersPlaces>(RelQ, new { UserId = userId, PlaceId = PlaceExists._id }).FirstOrDefault();
      
                    if (RelExists != null)
                    {
                        // -> if user has relationship, return
                        System.Console.WriteLine($"Relationship already added for user: {userId} & place: {PlaceExists._id}");
                    }
                    else
                    {
                        // -> if user DOES NOT, add new users_places entry
                        string NewRelQ = @"
                            INSERT INTO users_places (users__id, places__id)
                            VALUES (@UserId, @PlaceId)
                        ";
                        dbConnection.Execute(NewRelQ, new { UserId = userId, PlaceId = PlaceExists._id });
                    }
                }
                else
                {
                    // if place doesnt exist, add the place and add the relationship
                    string InsertPlaceQ = @"
                        INSERT INTO places (place_id, name, formatted_address, formatted_phone_number, rating, created_at, updated_at)
                        VALUES (@PlaceId, @Name, @FAddr, @FPhoneNumber, @Rating, NOW(), NOW())
                    ";
                    dbConnection.Execute(InsertPlaceQ, new {
                        PlaceId = place.place_id,
                        Name = place.name,
                        FAddr = place.formatted_address,
                        FPhoneNumber = place.formatted_phone_number,
                        Rating = place.rating,
                    });

                    // query for the new place, use ExistsQ from start of function
                    var NewPlace = dbConnection.Query<Place>(ExistsQ, new { PlaceAddr = place.formatted_address }).FirstOrDefault();

                    // add users_places relationship
                    string AddRelQ = @"
                        INSERT INTO users_places (users__id, places__id)
                        VALUES (@UserId, @PlaceId)
                    ";
                    dbConnection.Execute(AddRelQ, new { UserId = userId, PlaceId = NewPlace._id });

                    // add NewPlace.types / newPlace.hours / NewPlace.photos onto NewPlace
                    addAllPlaceChildData(NewPlace, place);
                }
            }
        }

        public void addAllPlaceChildData(Place NewPlace, PlaceResults PlaceData)
        {
            // INSERT TYPES FOR PLACE.TYPES
            using (IDbConnection dbConnection = Connection)
            {
                string InsertTypesQ = @"
                    INSERT INTO types (text, places__id, created_at, updated_at)
                    VALUES (@Text, @PlacesId, NOW(), NOW())
                ";

                dbConnection.Open();

                List<object> Types = new List<object>();
                foreach (var type in PlaceData.types)
                {
                    Types.Add(new { Text = type, PlacesId = NewPlace._id });
                }
                dbConnection.Execute(InsertTypesQ, Types);
                
                // INSERT HOURS FOR PLACE.HOURS
                string InsertHoursQ = @"
                    INSERT INTO hours (order_pos, text, places__id, created_at, updated_at)
                    VALUES (@OrderPos, @Text, @PlacesId, NOW(), NOW())
                ";
                List<object> Hours = new List<object>();
                // need to handle case where the place doesnt have opening_hours
                if (PlaceData.opening_hours == null)
                {
                    Hours.Add(new { OrderPos = 0, Text = "No hours specified", PlacesId = NewPlace._id });
                }
                else
                {
                    // case where there IS opening_hours
                    for (int i = 0; i < PlaceData.opening_hours.weekday_text.Length; i++)
                    {
                        Hours.Add(new { OrderPos = i, Text = PlaceData.opening_hours.weekday_text[i], PlacesId = NewPlace._id });
                    }
                }
                dbConnection.Execute(InsertHoursQ, Hours);

                // INSERT PHOTOS FOR PLACE.PHOTOS
                string InsertPhotosQ = @"
                    INSERT INTO photos (reference, places__id, created_at, updated_at)
                    VALUES (@Reference, @PlacesId, NOW(), NOW())
                ";
                List<object> Photos = new List<object>();
                if (PlaceData.photos != null)
                {
                    // only add photos if PlaceData.photos exists
                    foreach (var photo in PlaceData.photos)
                    {
                        Photos.Add(new { Reference = photo.photo_reference, PlacesId = NewPlace._id });
                    }
                    dbConnection.Execute(InsertPhotosQ, Photos);
                }
            }
        }
    }
}