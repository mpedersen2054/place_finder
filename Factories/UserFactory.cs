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

        // find by ID
        public User FindById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                string Query = @"
                    SELECT * FROM users
                    WHERE id = @Id
                ";
                return dbConnection.Query<User>(Query, new { Id = id }).FirstOrDefault();
            }
        }

        // if the user exists return the User
        // if user doesnt exist, create new User and return new User
        public User FindOrCreate(string Uname)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string QueryName = @"
                    SELECT * FROM users
                    WHERE name = @Name
                ";
                dbConnection.Open();
                var user = dbConnection.Query<User>(QueryName, new { Name = Uname }).FirstOrDefault();
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
                    dbConnection.Execute(QueryInsert, new { Name = Uname });
                    return dbConnection.Query<User>(QueryName, new { Name = Uname }).FirstOrDefault();
                }
            }
        }
    }
}