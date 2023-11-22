﻿using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Util;
using System.Data.SqlClient;

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
            Assert.That(con.State == System.Data.ConnectionState.Open);
            con.Close();
        }


    }
}