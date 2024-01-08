using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using System.Diagnostics;

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

        public void FillTable()
        {
            using SqlConnection con = DatabaseConnection.GetConnection();
            try
            {
                con.Open();
                int studentCounter = 1;
                string queryTotalStudent = "SELECT COUNT(*) as totalStudents " +
                    "FROM student; ";
                int totalStudents = 0;
                using (SqlCommand command = new SqlCommand(queryTotalStudent, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            totalStudents = Convert.ToInt32(reader["totalStudents"]);
                        }
                    }
                }
                while (studentCounter < totalStudents)
                {
                    string query = "INSERT INTO registrationSemester (studentId, semesterName) " +
                     "VALUES (@Id, @semesterName1); " +
                     "INSERT INTO registrationSemester (studentId, semesterName) " +
                     "VALUES (@Id, @semesterName2);";
                    string studentId = $"S{studentCounter.ToString($"D{6}")}";

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@Id", studentId);
                        command.Parameters.AddWithValue("@SemesterName1", "Basic programming 1");
                        command.Parameters.AddWithValue("@SemesterName2", "Basic programming 2");
                        command.ExecuteNonQuery();
                    }
                    studentCounter++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
            finally
            {
                if (con.State != System.Data.ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }

        public List<SemesterRegistration> GetAllSemesterRegistration()
        {
            string query = "SELECT * FROM registrationSemester";
            List<SemesterRegistration> registrationSemesters = new List<SemesterRegistration>();
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    if (connection.State != System.Data.ConnectionState.Open) { connection.Open(); };
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
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
                finally
                {
                    DatabaseConnection.CloseConnection(connection);
                }
            }
            return registrationSemesters;
        }
        public static void CreateRegistrationByStudentIdBasedOnSemester(string studentId, string semesterName)
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
                    command.Parameters.AddWithValue("@SemesterName", semesterName);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
            finally
            {
                DatabaseConnection.CloseConnection(con);
            }
        }

        public bool IsEnrolledForSemesterByStudentId(SqlConnection connection, string studentID)
        {
            string query = "SELECT * FROM registrationSemester WHERE studentId = @studentid";
            bool isEnrolled = false;
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
            return isEnrolled;
        }

        public bool IsEnrolledForSemesterByStudentId(SqlConnection connection, string studentID, Semester semester)
        {
            string query = "SELECT * FROM registrationSemester WHERE studentId = @studentid AND semesterName = @SemesterName";
            bool isEnrolled = false;
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
            return isEnrolled;
        }

        public static void UnsubscribeFromSemesterByStudentId(SqlConnection connection, string studentID, string semesterName)
        {
            string query = "DELETE FROM registrationSemester WHERE studentId = @studentId AND semesterName = @semesterName";

            if (connection.State != System.Data.ConnectionState.Open) { connection.Open(); };
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@studentId", studentID);
                command.Parameters.AddWithValue("@semesterName", semesterName);
                command.ExecuteNonQuery();
            }

        }

    }
}
