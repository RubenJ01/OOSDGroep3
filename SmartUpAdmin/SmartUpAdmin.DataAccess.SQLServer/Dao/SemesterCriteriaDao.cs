using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using SmartUpAdmin.DataAccess.SQLServer.Model;
using System.Diagnostics;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class SemesterCriteriaDao
    {
        private static SemesterCriteriaDao? instance;
        private SemesterCriteriaDao()
        {
        }

        public static SemesterCriteriaDao GetInstance()
        {
            if (instance == null)
            {
                instance = new SemesterCriteriaDao();
            }
            return instance;
        }


        public void FillTable()
        {
            using SqlConnection con = DatabaseConnection.GetConnection();
            try
            {
                con.Open();
                Random random = new Random();

                List<String> semesterAbbreviations = SemesterDao.GetInstance().GetAllSemesterAbbreviations();
                List<String> courseNames = CourseDao.GetInstance().GetAllCourseNames();

                for (int i = 0; i < 7; i++) // Choose 7 random semesters
                {
                    string randomSemesterAbbreviation = semesterAbbreviations[random.Next(semesterAbbreviations.Count)];

                    // Decide the number of random semester criteria to add (1 or 2)
                    int numberOfCriteria = random.Next(1, 3);

                    for (int j = 0; j < numberOfCriteria; j++)
                    {
                        string randomCourseName = courseNames[random.Next(courseNames.Count)];

                        string query = "INSERT INTO semesterCriteria (semesterAbbreviation, courseName) " +
                            "VALUES (@SemesterAbbreviation, @CourseName)";

                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.Parameters.AddWithValue("@SemesterAbbreviation", randomSemesterAbbreviation);
                            command.Parameters.AddWithValue("@CourseName", randomCourseName);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message} {ex.Source.ToUpper()}");
            }
            finally
            {
                if (con.State != System.Data.ConnectionState.Closed)
                {
                    DatabaseConnection.CloseConnection(con);
                }
            }
        }

        public List<SemesterCourse> GetAllSemestersCriteria()
        {
            string query = "SELECT * FROM semesterCriteria";
            List<SemesterCourse> semesters = new List<SemesterCourse>();
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
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
                                string abbreviation = reader["semesterAbbreviation"].ToString();
                                string semesterCourse = reader["courseName"].ToString();
                                semesters.Add(new SemesterCourse(abbreviation, semesterCourse));
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
            return semesters;
        }

        public List<SemesterCourse> GetSemesterCriteriaBySemester(Semester semester)
        {
            string query = "SELECT * FROM semesterCriteria WHERE semesterName = @semesterName";
            List<SemesterCourse> semesters = new List<SemesterCourse>();
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@semesterName", semester.Name);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string abbreviation = reader["semesterName"].ToString();
                                string semesterCourse = reader["courseName"].ToString();
                                semesters.Add(new SemesterCourse(abbreviation, semesterCourse));
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
            return semesters;
        }

        public void AddSemesterCriteria(SemesterCriteria semesterCriteria)
        {
            string query = "INSERT INTO semesterCriteria (semesterName, courseName) " +
                "VALUES (@SemesterName, @CourseName)";
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SemesterName", semesterCriteria.SemesterName);
                        command.Parameters.AddWithValue("@CourseName", semesterCriteria.CourseName);
                        command.ExecuteNonQuery();
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
        }
    }
}
