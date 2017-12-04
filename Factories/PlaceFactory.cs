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
                System.Console.WriteLine(place);
                System.Console.WriteLine(place.name);
                System.Console.WriteLine(place.place_id);
                // System.Console.WriteLine($"creating {place["name"]} for user: {userId}");
                dbConnection.Open();
                // check if the place already exists
                var ExistsQ = @"
                    SELECT * FROM places
                    WHERE place_id = @PlaceId
                ";
                // var Exists = dbConnection.Query<Place>(ExistsQ, new { PlaceId = place["place_id"] }).FirstOrDefault();

                // if (Exists != null)
                // {
                //     System.Console.WriteLine("DO SOMETHING IF IT EXISTS");
                // }
                // else
                // {
                //     string InsertQ = @"

                //     ";
                // }
                // if place doesnt exist add the place into the DB
            }
        }
    }
}