using SmartUp.DataAccess.SQLServer.Util;
using System;
using System.Data.SqlClient;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class TableCreator
    {
  

        public static void CreateTablesIfNotExist()
        {
            CreateTableIfNotExists("course",
                @"
                CREATE TABLE course (
                    [name] varchar(32),
                    credits int NOT NULL,
                    PRIMARY KEY([name])
                );"
            );

            CreateTableIfNotExists("upcomingExam",
                @"
                CREATE TABLE upcomingExam (
                    studentId varchar(32),
                    courseName varchar(32),
                    [date] DATETIME NOT NULL,
                    PRIMARY KEY (studentId, courseName),
                    FOREIGN KEY (courseName) REFERENCES course([name])
                );"
            );
        }

        private static void CreateTableIfNotExists(string tableName, string query)
        {
            using SqlConnection con = DatabaseConnection.GetConnection();
            try
            {
                con.Open();
                string checkTableQuery = $"IF OBJECT_ID('{tableName}', 'U') IS NULL";
                string createTableQuery = $"{checkTableQuery} BEGIN {query} END;";
                ExecuteNonQuery(createTableQuery, con);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (con.State != System.Data.ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }

        private static void ExecuteNonQuery(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
