using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using System.Data.SqlClient;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class GradeDao
    {
        private static GradeDao? instance;
        private GradeDao()
        {
        }

        public static GradeDao GetInstance()
        {
            if (instance == null)
            {
                instance = new GradeDao();
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

                List<String> studentIds = StudentDao.GetInstance().GetAllStudentIds();
                List<String> courseNames = CourseDao.GetInstance().GetAllCourseNames();
                foreach (string studentId in studentIds)
                {
                    List<String> selectedCourses = courseNames.Distinct().Take(random.Next(5) + 1).ToList();
                    foreach (string courseName in selectedCourses)
                    {
                        int randomInt = random.Next((int)(1.0m / 0.1m), (int)(10.0m / 0.1m));
                        decimal randomGrade = randomInt * 0.1m;;
                        DateTime startDate = new DateTime(2000, 1, 1);
                        DateTime endDate = new DateTime(2022, 12, 31);
                        int range = (endDate - startDate).Days;
                        int randomDays = random.Next(range);
                        TimeSpan randomTimeSpan = new TimeSpan(randomDays, random.Next(24), random.Next(60), random.Next(60));
                        DateTime randomDateTime = startDate + randomTimeSpan;
                        string query = "INSERT INTO grade (studentId, courseName, attempt, grade, isDefinitive, date) " +
                            "VALUES (@StudentId, @CourseName, @Attempt, @Grade, @IsDefinitive, @Date)";
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.Parameters.AddWithValue("@StudentId", studentId);
                            command.Parameters.AddWithValue("@CourseName", courseName);
                            command.Parameters.AddWithValue("@Attempt", 1);
                            command.Parameters.AddWithValue("@Grade", randomGrade);
                            command.Parameters.AddWithValue("@IsDefinitive", 0);
                            command.Parameters.AddWithValue("@Date", randomDateTime);
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

        public List<Grade> GetGradesByStudentId(string studentId)
        {
            List<Grade> grades = new List<Grade>();
            string query = "SELECT grade.grade, grade.isDefinitive, grade.date, grade.courseName, course.credits " +
                           "FROM grade JOIN course ON course.name = grade.courseName " +
                           "WHERE grade.studentId = @StudentId";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentId", studentId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                decimal grade = Convert.ToDecimal(reader["grade"]);
                                bool isDefinitive = Convert.ToBoolean(reader["isDefinitive"]);
                                DateTime date = Convert.ToDateTime(reader["date"]);
                                string courseName = reader["courseName"].ToString();
                                int credits = Convert.ToInt32(reader["credits"]);

                                grades.Add(new Grade(grade, isDefinitive, date, courseName, credits));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            return grades;
        }

    }
}
