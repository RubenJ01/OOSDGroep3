using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using System.Diagnostics;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class SemesterCourseDao
    {
        private static SemesterCourseDao? instance;
        private SemesterCourseDao()
        {
        }

        public static SemesterCourseDao GetInstance()
        {
            if (instance == null)
            {
                instance = new SemesterCourseDao();
            }
            return instance;
        }

        public List<SemesterCourse> GetAllSemesters()
        {
            string query = "SELECT * FROM semesterCourse";
            List<SemesterCourse> semesters = new List<SemesterCourse>();
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
        
        public List<string> GetSemesterCoursesBySemesterAbbreviation(string semesterAbbreviation)
        {
            string query = "SELECT courseName FROM semesterCourse WHERE semesterAbbreviation = @SemesterAbbreviation";
            List<string> semestersCourses = new List<string>();
            using(SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SemesterAbbreviation", semesterAbbreviation);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string semesterCourse = reader["courseName"].ToString();
                                semestersCourses.Add(semesterCourse);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return semestersCourses;
        }
    }
}
