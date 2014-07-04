using MySql.Data.MySqlClient;
using System;
namespace Phoenix.Storage
{
    internal class DatabaseManager
    {
        public Phoenix.Storage.Database Database;
        public DatabaseServer Server;

        public DatabaseManager(DatabaseServer _Server, Phoenix.Storage.Database _Database)
        {
            this.Server = _Server;
            this.Database = _Database;
        }

        public DatabaseClient GetClient()
        {
            return new DatabaseClient(this);
        }

        public string ConnectionString
        {
            get
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
                {
                    Server = this.Server.Host,
                    Port = this.Server.Port,
                    UserID = this.Server.User,
                    Password = this.Server.Password,
                    Database = this.Database.Name,
                    MinimumPoolSize = this.Database.minPoolSize,
                    MaximumPoolSize = this.Database.maxPoolSize,
                    Pooling = true,
                    AllowZeroDateTime = true,
                    ConvertZeroDateTime = true,
                    DefaultCommandTimeout = 30,
                    ConnectionTimeout = 10
                };
                return builder.ToString();
            }
        }
    }
}
