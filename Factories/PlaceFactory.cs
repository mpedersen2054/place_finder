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
                // System.Console.WriteLine(place);
                // System.Console.WriteLine(place.name);
                // System.Console.WriteLine(place.place_id);
                // System.Console.WriteLine(place.opening_hours);
                // System.Console.WriteLine(place.opening_hours.is_open);
                // System.Console.WriteLine(place.opening_hours.weekday_text);
                // System.Console.WriteLine(place.types);
                // System.Console.WriteLine(place.photos);

                dbConnection.Open();
                // check if the place already exists
                var ExistsQ = @"
                    SELECT * FROM places
                    WHERE place_id = @PlaceId
                ";
                var Exists = dbConnection.Query<Place>(ExistsQ, new { PlaceId = place.place_id }).FirstOrDefault();

                if (Exists != null)
                {
                    System.Console.WriteLine("DO SOMETHING IF IT EXISTS");
                    System.Console.WriteLine(Exists.place_id);
                }
                else
                {
                    int isOpen = place.opening_hours.is_open ? 1 : 0;
                    string InsertPlaceQ = @"
                        INSERT INTO places (place_id, formatted_address, formatted_phone_number, name, is_open, users__id, created_at, updated_at)
                        VALUES (@PlaceId, @FAddr, @FPhoneNumber, @Name, @IsOpen, @UsersId, NOW(), NOW())
                    ";
                    dbConnection.Execute(InsertPlaceQ, new {
                        PlaceId = place.place_id,
                        FAddr = place.formatted_address,
                        FPhoneNumber = place.formatted_phone_number,
                        Name = place.name,
                        IsOpen = isOpen,
                        UsersId = userId
                    });

                    // get the newly added place
                    var _Place = dbConnection.Query<Place>(ExistsQ, new { PlaceId = place.place_id }).FirstOrDefault();

                    System.Console.WriteLine("Heres the place");
                    System.Console.WriteLine(_Place);
                    System.Console.WriteLine(_Place.name);
                    System.Console.WriteLine(_Place.place_id);

                    // add types
                    string InsertTypesQ = @"
                        INSERT INTO types (text, places__id, created_at, updated_at)
                        VALUES (@Text, @PlacesId, NOW(), NOW())
                    ";
                    List<object> Types = new List<object>();
                    foreach (var type in place.types)
                    {
                        Types.Add(new { Text = type, PlacesId = _Place._id });
                    }
                    
                    dbConnection.Execute(InsertTypesQ, Types);
                    
                    // add open_hours
                    // add photos
                }
                // if place doesnt exist add the place into the DB
            }
        }
    }
}