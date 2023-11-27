using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using System.Diagnostics;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class SemesterDao
    {
        private static SemesterDao? instance;
        private SemesterDao()
        {
        }

        public static SemesterDao GetInstance()
        {
            if (instance == null)
            {
                instance = new SemesterDao();
            }
            return instance;
        }

        public void FillTable()
        {
            using SqlConnection con = DatabaseConnection.GetConnection();
            try
            {
                con.Open();
                Dictionary<string, (string Abbreviation, string Description)> itSemesters = new Dictionary<string, (string, string)>
                {
                    { "Introduction to Programming", ("ITP", "Programming fundamentals and basic algorithms") },
                    { "Data Structures and Algorithms", ("DSA", "Advanced data structures and algorithm analysis") },
                    { "Object-Oriented Programming", ("OOP", "Principles of object-oriented design and programming") },
                    { "Database Management Systems", ("DBMS", "Relational database concepts and SQL") },
                    { "Web Development and Design", ("WDD", "Front-end and back-end web development") },
                    { "Software Engineering Principles", ("SEP", "Software development life cycle and best practices") },
                    { "Networking and Security", ("NS", "Computer networking and information security") },
                    { "Artificial Intelligence and Machine Learning", ("AIML", "Fundamentals of AI and machine learning") },
                    { "Cloud Computing Technologies", ("CCT", "Cloud infrastructure and services") },
                    { "Capstone Project and Final Assessment", ("CPFA", "Integrated project to demonstrate skills and knowledge") }
                };

                Random random = new Random();
                foreach (var semester in itSemesters)
                {
                    string name = semester.Key;
                    string abbreviation = semester.Value.Abbreviation;
                    string description = semester.Value.Description;
                    int randomNumber = random.Next(1, 101);
                    int requiredCredits;
                    if (randomNumber <= 75)
                    {
                        requiredCredits = 0;
                    }
                    else if (randomNumber <= 87)
                    {
                        requiredCredits = 45;
                    }
                    else
                    {
                        requiredCredits = 60;
                    }
                    string insertQuery = "INSERT INTO semester (name, abbreviation, description, requiredCreditsFromP) " +
                        "VALUES(@name, @abbreviation, @description, @requiredCreditsFromP)";
                    using (SqlCommand command = new SqlCommand(insertQuery, con))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@abbreviation", abbreviation);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@requiredCreditsFromP", requiredCredits);

                        command.ExecuteNonQuery();
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

        public List<String> GetAllSemesterAbbreviations()
        {
            string query = "SELECT abbreviation FROM semester;";
            List<string> abbreviations = new List<string>();
            using (SqlConnection connection = DatabaseConnection.GetConnection())
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
                                string abbreviation = reader["abbreviation"].ToString();
                                abbreviations.Add(abbreviation);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return abbreviations;
        }

        public List<Semester> GetAllSemesters()
        {
            string query = "SELECT * FROM semester";
            List<Semester> semesters = new List<Semester>();
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
                                string name = reader["name"].ToString();
                                string abbreviation = reader["abbreviation"].ToString();
                                string description = reader["description"].ToString();
                                int requiredCreditsFromP = Int32.Parse(reader["requiredCreditsFromP"].ToString());
                                semesters.Add(new Semester(name, abbreviation, description, requiredCreditsFromP));
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
