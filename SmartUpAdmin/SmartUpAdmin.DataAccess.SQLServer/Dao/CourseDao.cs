using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class CourseDao
    {
        private static CourseDao? instance;
        private CourseDao()
        {
        }

        public static CourseDao GetInstance()
        {
            if (instance == null)
            {
                instance = new CourseDao();
            }
            return instance;
        }

        public void FillTable()
        {
            List<string> itCourses = new List<string>
            {
                "Introduction to Computer Science",
                "Data Structures and Algorithms",
                "Web Development Fundamentals",
                "Database Management Systems",
                "Network Fundamentals",
                "Cybersecurity Basics",
                "Software Engineering Principles",
                "Mobile App Development",
                "Cloud Computing",
                "Artificial Intelligence and Machine Learning",
                "DevOps Practices",
                "IT Project Management",
                "Computer Graphics and Visualization",
                "Human-Computer Interaction",
                "Game Development",
                "Blockchain Technology",
                "Internet of Things (IoT)",
                "Big Data Analytics",
                "Digital Marketing for IT Professionals",
                "IT Ethics and Cybersecurity Policies",
                "Operating Systems Concepts",
                "Computer Architecture",
                "Computer Networks",
                "Software Testing and Quality Assurance",
                "Object-Oriented Programming",
                "Web Design and User Experience",
                "Linux System Administration",
                "Windows Server Administration",
                "Data Warehousing and Data Mining",
                "Computer Forensics",
                "Network Security",
                "Advanced Web Technologies",
                "Embedded Systems Programming",
                "Computer Vision",
                "Quantum Computing",
                "Mobile Game Development",
                "Cloud Security",
                "Natural Language Processing",
                "IT Governance and Compliance",
                "IT Service Management",
                "Parallel and Distributed Computing",
                "Wireless Communication",
                "IT Strategy and Planning",
                "Biometric Systems",
                "Virtual Reality and Augmented Reality",
                "IT Disaster Recovery Planning",
                "Health Informatics",
                "IT for Sustainable Development"
            };
            using SqlConnection con = DatabaseConnection.GetConnection();
            try
            {
                con.Open();
                Random random = new Random();
                foreach (string courseName in itCourses)
                {
                    int credits = random.Next(1, 6);
                    string query = "INSERT INTO course ([name], credits) VALUES (@name, @credits);";
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@name", courseName);
                        command.Parameters.AddWithValue("@credits", credits);
                        command.ExecuteNonQuery();
                    }
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

        public List<String> GetAllCourseNames()
        {
            List<String> courseNames = new List<string>();
            String query = "SELECT [name] FROM course;";
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
                                string courseName = reader["name"].ToString();
                                courseNames.Add(courseName);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
            }
            return courseNames;
        }

        public void AddNewCourse(string name, int credits)
        {
            string query = "INSERT INTO course (name, credits) VALUES (@name, @credits)";
            
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                 try
                 {
                    using (SqlCommand command = new SqlCommand(query, connection))
                     {
                            command.Parameters.AddWithValue("@name", name);
                            command.Parameters.AddWithValue("@credits", credits);

                            command.ExecuteNonQuery();
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                    }
                
            }
        }

        public Course GetCourseByCourseName(string name)
        {
            string query = "SELECT * FROM course WHERE name = @name";
            Course? course = null;
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string courseName = reader["name"].ToString();
                                int credits = Int32.Parse(reader["credits"].ToString());
                                course = new Course(courseName, credits);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
            }
            return course;
        }

        public void AddCourseToSemester(string name, string semester)
        {
            string query = "INSERT INTO semesterCourse (semesterName, courseName) VALUES (@semester, @name)";

            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
               connection.Open();
                  try
                  {
                      using (SqlCommand command = new SqlCommand(query, connection))
                      {
                        command.Parameters.AddWithValue("@semester", semester);
                        command.Parameters.AddWithValue("@name", name);
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
