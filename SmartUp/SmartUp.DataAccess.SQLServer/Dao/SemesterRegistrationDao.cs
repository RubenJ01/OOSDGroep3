using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class SemesterRegistrationDao
    {
        private static SemesterRegistrationDao? instance;
        private SemesterRegistrationDao()
        {
        }

        public static SemesterRegistrationDao GetInstance()
        {
            if (instance == null)
            {
                instance = new SemesterRegistrationDao();
            }
            return instance;
        }
        
        public List<SemesterRegistration> GetAllSemesterRegistration()
        {
            string query = "SELECT * FROM registrationSemester";
            List<SemesterRegistration> registrationSemesters = new List<SemesterRegistration>();
            using(SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string studentId = reader["studentId"].ToString();
                                string abbreviation = reader["abbreviation"].ToString();
                                registrationSemesters.Add(new SemesterRegistration(abbreviation, studentId));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return registrationSemesters;
        }
        public static void CreateRegistrationByStudentIdBasedOnSemester(string studentId, string abbreviation)
        {
            using SqlConnection con = DatabaseConnection.GetConnection();
            try
            {
                con.Open();

                string query = "INSERT INTO registrationSemester (studentId, abbreviation) " +
                "VALUES (@StudentId, @Abbreviation)";
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.Parameters.AddWithValue("@StudentId", studentId);
                            command.Parameters.AddWithValue("@Abbreviation", abbreviation);
                            command.ExecuteNonQuery();
                        }
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

    }
}
