using Microsoft.Data.SqlClient;
using SmartUp.Core.Util;
using SmartUp.DataAccess.SQLServer.Util;
using System.Data.SqlClient;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class StudentDao
    {
        private static StudentDao? instance;
        private StudentDao()
        {
        }

        public static StudentDao GetInstance()
        {
            if (instance == null)
            {
                instance = new StudentDao();
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
                int totalStudents = 1;
                List<String> classNames = ClassDao.GetInstance().GetClassNames();
                List<String> mentors = TeacherDao.GetInstance().GetAllMentorIds();
                foreach (string className in classNames)
                {
                    int studentsInClass = random.Next(20, 30);
                    for (int i = 0; i < studentsInClass; i++)
                    {
                        string firstName = NameGenerator.GenerateRandomName();
                        string infix = "";
                        if (random.Next(5) == 3)
                        {
                            infix = NameGenerator.GenerateRandomInfix();
                        }
                        string lastName = NameGenerator.GenerateRandomName();
                        string studentId = $"S{totalStudents.ToString($"D{6}")}";
                        string mentorId = mentors[random.Next(mentors.Count)];
                        string insertQuery = "INSERT INTO student (id, firstName, infix, lastName, mentor, totalCredits, totalCreditsFromP, class) " +
                                             "VALUES (@Id, @FirstName, @Infix, @LastName, @Mentor, @TotalCredits, @TotalCreditsFromP, @Class);";

                        using (SqlCommand command = new SqlCommand(insertQuery, con))
                        {
                            command.Parameters.AddWithValue("@Id", studentId);
                            command.Parameters.AddWithValue("@FirstName", firstName);
                            command.Parameters.AddWithValue("@Infix", infix);
                            command.Parameters.AddWithValue("@LastName", lastName);
                            command.Parameters.AddWithValue("@Mentor", mentorId);
                            command.Parameters.AddWithValue("@TotalCredits", 0);
                            command.Parameters.AddWithValue("@TotalCreditsFromP", 0);
                            command.Parameters.AddWithValue("@Class", className);
                            command.ExecuteNonQuery();
                        }
                        totalStudents++;
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


        public List<String> GetAllStudentIds()
        {
            List<String> studentIds = new List<string>();
            String query = "SELECT id FROM student;";
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
                                string id = reader["id"].ToString();
                                studentIds.Add(id);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return studentIds;
        }

        public int GetCreditsFromPByStudentID(string studentID)
        {
            int creditsFromP = 0;
            String query = "SELECT totalCreditsFromP FROM student WHERE id = @StudentID;";
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentID", studentID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Int32.Parse(reader["totalCreditsFromP"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return creditsFromP;
        }
    }
}
