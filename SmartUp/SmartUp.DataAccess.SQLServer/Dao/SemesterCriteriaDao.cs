using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
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

                for (int i = 0; i < 7; i++)
                {
                    string randomSemesterAbbreviation = semesterAbbreviations[random.Next(semesterAbbreviations.Count)];

                    int numberOfCriteria = random.Next(1, 3);

                    for (int j = 0; j < numberOfCriteria; j++)
                    {
                        string randomCourseName = courseNames[random.Next(courseNames.Count)];

                        string query = "INSERT INTO semesterCriteria (semesterName, courseName) " +
                            "VALUES (@SemesterName, @CourseName)";

                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.Parameters.AddWithValue("@SemesterName", randomSemesterAbbreviation);
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
                    con.Close();
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
                                string name = reader["semesterName"].ToString();
                                string semesterCourse = reader["courseName"].ToString();
                                semesters.Add(new SemesterCourse(name, semesterCourse));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
            }
            return semesters;
        }

        public List<SemesterCourse> GetSemesterCriteriaBySemester(Semester semester)
        {
            string query = "SELECT * FROM semesterCriteria WHERE semesterName = @SemesterName";
            List<SemesterCourse> semesters = new List<SemesterCourse>();
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SemesterName", semester.Name);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader["semesterName"].ToString();
                                string semesterCourse = reader["courseName"].ToString();
                                semesters.Add(new SemesterCourse(name, semesterCourse));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
            }
            return semesters;
        }
    }
}
