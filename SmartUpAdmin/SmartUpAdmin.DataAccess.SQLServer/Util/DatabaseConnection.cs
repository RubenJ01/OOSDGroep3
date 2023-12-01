﻿using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace SmartUp.DataAccess.SQLServer.Util
{
    public static class DatabaseConnection
    {

        public static SqlConnection? GetConnection()
        {
            try
            {
                Config databaseConfig = Config.GetInstance();
                string server = databaseConfig.Server;
                string database = databaseConfig.Database;
                string username = databaseConfig.User;
                string password = databaseConfig.Password;
                string connectionUrl = $"Data Source={server};Initial Catalog={database};Integrated Security=True; TrustServerCertificate=True; Connect Timeout = 5;";

                return new SqlConnection(connectionUrl);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during database connection setup: {ex.Message}");
            }
            return null;
        }

    }
}
