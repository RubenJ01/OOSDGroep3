using Microsoft.Data.SqlClient;
using Renci.SshNet;
using System.Diagnostics;

namespace SmartUp.DataAccess.SQLServer.Util
{
    public static class DatabaseConnection
    {
        private static Config config = Config.GetInstance();
        private static SshClient? _sshClient;
        private static ForwardedPortLocal? _sshPortForwarding;
        
        public static SqlConnection? GetConnection()
        {
            if (config.UseServer)
            {
                try
                {
                    EstablishSshTunnel();
                    SqlConnectionStringBuilder builder = BuildSqlConnectionString();
                    return new SqlConnection(builder.ConnectionString);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
            }
            else
            {
                try
                {
                    string connectionUrl = CreateConnectionUrl();
                    return new SqlConnection(connectionUrl);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during database connection setup: {ex.Message}");
                }
            }
            return null;
        }

        private static string CreateConnectionUrl()
        {
            string server = config.LocalServer;
            string database = config.LocalDatabase;
            string connectionUrl = $"Data Source={server};Initial Catalog={database};Integrated Security=True; TrustServerCertificate=True; Connect Timeout = 5;";
            return connectionUrl;
        }

        private static SqlConnectionStringBuilder BuildSqlConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = config.MssqlHost;
            builder.UserID = config.MssqlUsername;
            builder.Password = config.MssqlPassword;
            builder.InitialCatalog = config.SshServer;
            builder.TrustServerCertificate = config.TrustServerCertificate;
            return builder;
        }

        private static void EstablishSshTunnel()
        {
            _sshClient = new SshClient(config.SshHost, config.SshUsername, config.SshPassword);
            _sshClient.Connect();
            _sshPortForwarding = new ForwardedPortLocal(config.MssqlHost, (uint)config.MssqlPort, config.MssqlHost, (uint)config.MssqlPort);
            _sshClient.AddForwardedPort(_sshPortForwarding);
            _sshPortForwarding.Start();
        }

        public static void CloseConnection(SqlConnection sqlConnection)
        {
            sqlConnection.Close();
            if (config.UseServer)
            {
                CloseSshTunnel();
            }
        }

        public static void CloseSshTunnel()
        {
            _sshPortForwarding.Stop();
            _sshPortForwarding.Dispose();
            _sshClient.Disconnect();
            _sshClient.Dispose();

        }
    }
}

