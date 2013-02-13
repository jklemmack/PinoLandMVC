using System;
using System.Configuration;
using System.Data.EntityClient;


namespace Fuqua.CompetativeAnalysis.MarketGame
{
    public class Database
    {
        private static object _lockObject = new object();
        private static Database _singleton = null;
        private string _connectionString = null;

        private Database()
        {
            //Default to our internal connection string.
            _connectionString = GetGlobalConnectionString();
        }

        public static Database Instance
        {
            get
            {
                if (_singleton == null)
                {
                    lock (_lockObject)
                    {
                        if (_singleton == null)
                            _singleton = new Database();
                    }
                }
                return _singleton;
            }
        }

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }

        }

        public string DatabaseConnectionString
        {
            get
            {
                EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(_connectionString);
                return entityBuilder.ProviderConnectionString;
            }
            set
            {
                EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(_connectionString);
                entityBuilder.ProviderConnectionString = value;
                _connectionString = entityBuilder.ConnectionString;
            }
        }

        private string GetGlobalConnectionString()
        {

            ConnectionStringSettings connS = ConfigurationManager.ConnectionStrings["GameDataObjectContext"];
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(connS.ConnectionString);

            System.Data.SqlClient.SqlConnectionStringBuilder sqlBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(entityBuilder.ProviderConnectionString);
            sqlBuilder.ApplicationName = "PinoLand - Core Engine";
            entityBuilder.ProviderConnectionString = sqlBuilder.ConnectionString;

            //Update our global db connection
            return entityBuilder.ConnectionString;
        }
    }
}
