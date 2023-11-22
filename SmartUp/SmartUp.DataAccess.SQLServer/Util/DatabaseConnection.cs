using System.Data.SqlClient;

namespace SmartUp.DataAccess.SQLServer.Util 
{
    public static class DatabaseConnection
    {
        private static readonly SqlConnection Connection;

        static DatabaseConnection()
        {
            try
            {
                Config databaseConfig = Config.GetInstance();
                string server = databaseConfig.Server;
                string database = databaseConfig.Database;
                string username = databaseConfig.User;
                string password = databaseConfig.Password;
                string connectionUrl = $"Data Source={server};Initial Catalog={database};Integrated Security=True;";

                Connection = new SqlConnection(connectionUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during database connection setup: {ex.Message}");
            }
        }

        public static SqlConnection GetConnection() => Connection;

    }
}

