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
                List<String> selectedsemesterAbbreviations = semesterAbbreviations.Distinct().Take(random.Next(5) + 1).ToList();
                List<String> courseNames = CourseDao.GetInstance().GetAllCourseNames();
                foreach (string semesterAbbreviation in selectedsemesterAbbreviations)
                {
                    List<String> selectedCourses = courseNames.Distinct().Take(random.Next(3) - 1).ToList();
                    foreach (string courseName in selectedCourses)
                    {
                        string query = "INSERT INTO semesterCriteria (semesterAbbreviation, courseName) " +
                            "VALUES (@SemesterAbbreviation, @CourseName)";
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.Parameters.AddWithValue("@SemesterAbbreviation", semesterAbbreviation);
                            command.Parameters.AddWithValue("@CourseName", courseName);
                            command.ExecuteNonQuery();
                        }
                    }
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

        public List<SemesterCourse> GetAllSemesters()
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
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return semesters;
        }
    }
}
