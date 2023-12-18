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
                    "Artificial Intelligence and Machine Learning",
                    "Capstone Project and Final Assessment",
                    "Cloud Computing Technologies" ,
                    "Data Structures and Algorithms ",
                    "Database Management Systems ",
                    "Introduction to Programming",
                    "Networking and Security",
                    "Object-Oriented Programming",
                    "Software Engineering Principles",
                    "Web Development and Design"
                };
                int JumpToSemester = 0;
                for (int i = 0; i < (itCourses.Count - 1); i++)
                {
                    string SemesterName = itSemester[JumpToSemester];

                    string CourseName = itCourses[i];
                    Debug.WriteLine(CourseName);
                    string insertQuery = "INSERT INTO semesterCourse (semesterName, courseName) " +
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
                Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
            finally
            {
                if (con.State != System.Data.ConnectionState.Closed)
                {
                    DatabaseConnection.CloseConnection(con);
                }
            }
        }

        public List<SemesterCourse> GetAllSemesters()
        {
            string query = "SELECT * FROM semesterCourse";
            List<SemesterCourse> semesters = new List<SemesterCourse>();
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
        public List<string> GetSemesterCoursesBySemesterName(SqlConnection connection ,string semesterName)
        {
            string query = "SELECT courseName FROM semesterCourse WHERE semesterName = @SemesterName";
            List<string> semestersCourses = new List<string>();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SemesterName", semesterName);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string semesterCourse = reader["courseName"].ToString();
                        semestersCourses.Add(semesterCourse);
                    }
                }
            }
            return semestersCourses;
        }

        public Decimal GetPercentagePassed(SqlConnection? connection, string studentId, string semesterName)
        {
            connection.Open();
            Decimal percentagePassed = 0;
            string query = @"
WITH SemesterCourses AS (
    SELECT
        s.id AS StudentID,
        sc.semesterName,
        c.name AS CourseName,
        c.credits,
        g.grade,
        g.isDefinitive
    FROM
        student s
    JOIN registrationSemester rs ON s.id = rs.studentId
    JOIN semesterCourse sc ON rs.semesterName = sc.semesterName
    JOIN course c ON sc.courseName = c.name
    LEFT JOIN grade g ON s.id = g.studentId AND c.name = g.courseName
    WHERE
        s.id = @StudentID
        AND sc.semesterName = @Semester
),
WeightedSum AS (
    SELECT
        StudentID,
        semesterName,
        CAST(SUM(CASE WHEN grade >= 5.5 AND isDefinitive = 1 THEN credits ELSE 0 END) AS DECIMAL(10, 0)) AS TotalSumCreditsStudent
    FROM
        SemesterCourses
    GROUP BY
        StudentID, semesterName
),
TotalCreditsSemester AS (
    SELECT
        StudentID,
        semesterName,
        SUM(credits) AS TotalCreditsSemester
    FROM
        SemesterCourses
    GROUP BY
        StudentID, semesterName
)
SELECT
    CAST(ROUND((100.0 / tc.TotalCreditsSemester) * wc.TotalSumCreditsStudent, 0) AS DECIMAL(10, 0)) AS PercentagePassed
FROM
    WeightedSum wc
JOIN TotalCreditsSemester tc ON wc.StudentID = tc.StudentID AND wc.semesterName = tc.semesterName;
";

            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Semester", semesterName);
                command.Parameters.AddWithValue("@StudentID", studentId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        percentagePassed = Convert.ToDecimal(reader["PercentagePassed"].ToString());
                    }
                }
            }
            connection.Close();
            return percentagePassed;
        }
    }
}
