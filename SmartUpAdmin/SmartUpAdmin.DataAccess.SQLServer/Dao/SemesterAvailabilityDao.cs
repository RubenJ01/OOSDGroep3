using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Util;
using SmartUpAdmin.DataAccess.SQLServer.Model;
using System.Diagnostics;

namespace SmartUpAdmin.DataAccess.SQLServer.Dao
{
    public class SemesterAvailabilityDao
    {
        private static SemesterAvailabilityDao? instance;

        private SemesterAvailabilityDao()
        {
        }

        public static SemesterAvailabilityDao GetInstance() {
            if (instance == null)
            {
                instance = new SemesterAvailabilityDao();
            }
            return instance;
        }

        public void AddSemesterAvailability(SemesterAvailability semesterAvailability)
        {
            string query = "INSERT INTO SemesterAvailability (semesterName, availableInSemester) VALUES (@semesterName, @availableInSemester)";
            using SqlConnection con = DatabaseConnection.GetConnection();
            try
            {
                con.Open();
                
                using(SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@semesterName", semesterAvailability.SemesterName);
                    cmd.Parameters.AddWithValue("@availableInSemester", semesterAvailability.AvailableInSemester);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message} {ex.Source.ToUpper()}");
            }
        }   
    }
}
