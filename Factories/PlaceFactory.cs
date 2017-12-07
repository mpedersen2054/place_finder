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
                    WHERE place_id = @PlaceId
                ";
                string InsertPlaceQ;
                string InsertTypesQ;
                string InsertHoursQ;
                string InsertPhotosQ;
                // check if the place already exists
                var Exists = dbConnection.Query<Place>(ExistsQ, new { PlaceId = place.place_id }).FirstOrDefault();
                if (Exists != null && Exists.users__id == userId)
                {
                    System.Console.WriteLine("This place is already added for this user.");
                    System.Console.WriteLine(Exists.place_id);
                }
                else
                {
                    // INSERT NEW PLACE
                    // check if place has .opening_hours, not all places do
                    int isOpen;
                    if (place.opening_hours != null)
                    {
                        isOpen = place.opening_hours.is_open ? 1 : 0;
                    }
                    else
                    {
                        isOpen = 0;
                    }

                    InsertPlaceQ = @"
                        INSERT INTO places (place_id, formatted_address, formatted_phone_number, name, is_open, rating, users__id, created_at, updated_at)
                        VALUES (@PlaceId, @FAddr, @FPhoneNumber, @Name, @IsOpen, @Rating, @UsersId, NOW(), NOW())
                    ";
                    dbConnection.Execute(InsertPlaceQ, new {
                        PlaceId = place.place_id,
                        FAddr = place.formatted_address,
                        FPhoneNumber = place.formatted_phone_number,
                        Name = place.name,
                        IsOpen = isOpen,
                        Rating = place.rating,
                        UsersId = userId
                    });

                    // get the newly added place to insert into users__id in following insert statements
                    var _Place = dbConnection.Query<Place>(ExistsQ, new { PlaceId = place.place_id }).FirstOrDefault();

                    // INSERT TYPES FOR PLACE.TYPES
                    InsertTypesQ = @"
                        INSERT INTO types (text, places__id, created_at, updated_at)
                        VALUES (@Text, @PlacesId, NOW(), NOW())
                    ";
                    List<object> Types = new List<object>();
                    foreach (var type in place.types)
                    {
                        Types.Add(new { Text = type, PlacesId = _Place._id });
                    }
                    dbConnection.Execute(InsertTypesQ, Types);
                    
                    // INSERT HOURS FOR PLACE.HOURS
                    InsertHoursQ = @"
                        INSERT INTO hours (order_pos, text, places__id, created_at, updated_at)
                        VALUES (@OrderPos, @Text, @PlacesId, NOW(), NOW())
                    ";
                    List<object> Hours = new List<object>();
                    // need to handle case where the place doesnt have opening_hours
                    if (place.opening_hours == null)
                    {
                        Hours.Add(new { OrderPos = 0, Text = "No hours specified", PlacesId = _Place._id });
                    }
                    else
                    {
                        // case where there IS opening_hours
                        for (int i = 0; i < place.opening_hours.weekday_text.Length; i++)
                        {
                            Hours.Add(new { OrderPos = i, Text = place.opening_hours.weekday_text[i], PlacesId = _Place._id });
                        }
                    }
                    dbConnection.Execute(InsertHoursQ, Hours);

                    // INSERT PHOTOS FOR PLACE.PHOTOS
                    InsertPhotosQ = @"
                        INSERT INTO photos (reference, places__id, created_at, updated_at)
                        VALUES (@Reference, @PlacesId, NOW(), NOW())
                    ";
                    List<object> Photos = new List<object>();
                    if (place.photos != null)
                    {
                        // only add photos if place.photos exists
                        foreach (var photo in place.photos)
                        {
                            Photos.Add(new { Reference = photo.photo_reference, PlacesId = _Place._id });
                        }
                        dbConnection.Execute(InsertPhotosQ, Photos);
                    }
                }
            }
        }
    }
}