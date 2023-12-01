using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Util;

namespace SmartUp.Tests.DataAccess.SQLServer.Util
{
    [TestFixture]
    public class DatabaseConnectionTests
    {

        [Test]
        public void GetConnection_IsConnected()
        {
            SqlConnection con = DatabaseConnection.GetConnection();
            con.Open();
            Assert.That(con.State, Is.EqualTo(System.Data.ConnectionState.Open));
            con.Close();
        }
    }
}
