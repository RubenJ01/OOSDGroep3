using Microsoft.Data.SqlClient;
using Renci.SshNet;
using System;
using System.Diagnostics;

namespace SmartUp.DataAccess.SQLServer
{
    public class SmartUpRepository
    {
        public void StartDatabaseConnection(bool IsLocal)
        {
            if (IsLocal)
            {
                StartLocalDatabase();
            }
            else
            {
                StartSshDatabase();
            }
        }



        private void StartLocalDatabase()
        {

        }
        private void StartSshDatabase()
        {

            try
            {
                string sshHost = "145.44.233.146";
                string sshUsername = "student";
                string sshPassword = "OOSDDb3!";

                if (EstablishSshConnection(sshHost, sshUsername, sshPassword))
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private bool EstablishSshConnection(string sshHost, string sshUsername, string sshPassword)
        {
            using (var client = new SshClient(sshHost, sshUsername, sshPassword))
            {
                client.Connect();
                if (!client.IsConnected)
                {
                    Log("Kan geen SSH-verbinding tot stand brengen.");
                    return false;
                }
                Log("SSH-verbinding is tot stand gebracht.");
                var portForwarded = new ForwardedPortLocal("127.0.0.1", 1433, "localhost", 1433);
                client.AddForwardedPort(portForwarded);
                portForwarded.Start();
                return true;
            }
        }

        private void Log(string message)
        {
            Debug.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
        }
    }
}
