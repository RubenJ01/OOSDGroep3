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
        
        public static void CreateRegistrationByStudentIdBasedOnSemester(string studentId, string name)
        {
            using SqlConnection con = DatabaseConnection.GetConnection();
            try
            {
                con.Open();

                string query = "INSERT INTO registrationSemester (studentId, semesterName) " +
                "VALUES (@StudentId, @SemesterName)";
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.Parameters.AddWithValue("@StudentId", studentId);
                            command.Parameters.AddWithValue("@SemesterName", name);
                            command.ExecuteNonQuery();
                        }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

        public bool IsEnrolledForSemesterByStudentId(string studentID)
        {
            string query = "SELECT * FROM registrationSemester WHERE studentId = @studentid";
            bool isEnrolled = false;
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentid", studentID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isEnrolled = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
            }
            return isEnrolled;
        }

        public bool IsEnrolledForSemesterByStudentId(string studentID, Semester semester)
        {
            string query = "SELECT * FROM registrationSemester WHERE studentId = @studentid AND semesterName = @SemesterName";
            bool isEnrolled = false;
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentid", studentID);
                        command.Parameters.AddWithValue("@SemesterName", semester.Name);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isEnrolled = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
            }
            return isEnrolled;
        }

        public static void UnsubscribeFromSemesterByStudentId(string studentID)
        {
            string query = "DELETE FROM registrationSemester WHERE studentId = @studentid";
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentid", studentID);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
            }
        }

    }
}
