using System;
using Microsoft.Data.SqlClient;
using System.Text;
using Microsoft.Identity.Client.SSHCertificates;
using Renci.SshNet;

namespace TestDockerConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "127.0.0.1";
                builder.UserID = "SA";
                builder.Password = "OOSDDb3!";
                builder.InitialCatalog = "SmartUpDB";
                builder.TrustServerCertificate = true;

                // Verbindingsinformatie voor de SSH-server
                string sshHost = "145.44.233.146";
                string sshUsername = "student";
                string sshPassword = "OOSDDb3!"; // Voer het wachtwoord van de SSH-gebruiker in

                // Verbindingsinformatie voor de database (doel-IP en -poort)
                string dbHost = "localhost";
                int dbPortLocal = 1433;
                int dbPortRemote = 1433;

                // Maak een nieuwe SSH-clientverbinding aan
                using (var client = new SshClient(sshHost, sshUsername, sshPassword))
                {
                    // Start de SSH-verbinding
                    client.Connect();

                    if (client.IsConnected)
                    {
                        Console.WriteLine("SSH-verbinding is tot stand gebracht.");

                        // Maak een lokale poortkoppeling naar de doel-IP en -poort van de database
                        var portForwarded = new ForwardedPortLocal("127.0.0.1", (uint)dbPortLocal, dbHost, (uint)dbPortRemote);
                        client.AddForwardedPort(portForwarded);

                        // Start de poortkoppeling
                        portForwarded.Start();

                        using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                        {
                            Console.WriteLine("\nQuery data example:");
                            Console.WriteLine("=========================================\n");

                            String sql = "SELECT * FROM student";

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                connection.Open();
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Console.WriteLine("{0} {1}", reader.GetInt32(0), reader.GetString(1));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Kan geen SSH-verbinding tot stand brengen.");
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }
    }
}