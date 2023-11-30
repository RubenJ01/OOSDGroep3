using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using System.Diagnostics;
using System.Diagnostics.Metrics;

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
        public void FillTable()
        {
            using SqlConnection con = DatabaseConnection.GetConnection();
            try
            {
                con.Open();

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
                List<string> itSemester = new List<string>
                {
                    "ITP",
                    "DSA",
                    "OOP",
                    "DBMS",
                    "WDD",
                    "SEP",
                    "NS",
                    "AIML",
                    "CCT",
                    "CPFA"
                };
                int JumpToSemester = 0;
                for(int i = 0; i < (itCourses.Count -1) ; i++)
                {
                    string SemesterName = itSemester[JumpToSemester];

                    string CourseName = itCourses[i];
                    Console.WriteLine(CourseName);
                    string insertQuery = "INSERT INTO semesterCourse (semesterAbbreviation, courseName) " +
    "VALUES(@semester, @course)";
                    using (SqlCommand command = new SqlCommand(insertQuery, con))
                    {
                        command.Parameters.AddWithValue("@semester", SemesterName);
                        command.Parameters.AddWithValue("@course", CourseName);
                        

                        command.ExecuteNonQuery();
                    }
                    if (i != 0 && i % 5 == 0)
                    {
                        JumpToSemester++;
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
