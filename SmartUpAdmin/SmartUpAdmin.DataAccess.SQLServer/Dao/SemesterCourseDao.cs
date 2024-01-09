﻿using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using SmartUpAdmin.DataAccess.SQLServer.Model;
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
                for (int i = 0; i < (itCourses.Count - 1); i++)
                {
                    string SemesterName = itSemester[JumpToSemester];

                    string CourseName = itCourses[i];
                    Debug.WriteLine(CourseName);
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

        public List<SemesterCourse> GetSemesterCoursesBySemesterName(SqlConnection connection, string semesterName)
        {
            string query = "SELECT courseName FROM semesterCourse WHERE semesterName = @SemesterName";
            List<SemesterCourse> semestersCourses = new List<SemesterCourse>();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SemesterName", semesterName);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string courseName = reader["courseName"].ToString();
                        SemesterCourse semesterCourse = new SemesterCourse(semesterName, courseName);
                        semestersCourses.Add(semesterCourse);
                    }
                }
            }
            return semestersCourses;
        }

        public void AddSemesterCourse(SqlConnection connection, SemesterCourse semesterCourse)
        {
            string query = "INSERT INTO semesterCourse (semesterName, courseName) " +
                "VALUES(@SemesterName, @CourseName)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SemesterName", semesterCourse.SemesterName);
                command.Parameters.AddWithValue("@CourseName", semesterCourse.CourseName);
                command.ExecuteNonQuery();
            }
        }

        public SemesterCourse? GetSemesterCourseByName(SqlConnection? connection, string name)
        {
            string query = "SELECT * FROM semesterCourse " +
                "WHERE courseName = @CourseName";
            SemesterCourse? semesterCourse = null;
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CourseName", name);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string courseName = reader["courseName"].ToString();
                        string semesterName = reader["semesterName"].ToString();
                        semesterCourse = new SemesterCourse(semesterName, courseName);
                    }
                }
            }
            return semesterCourse;
        }

        public void UpdateSemesterCourse(SqlConnection connection, List<SemesterCourse> semesterCoursesOld, List<SemesterCourse> semesterCoursesNew)
        {
            List<SemesterCourse> courseToDelete = new List<SemesterCourse>();
            List<SemesterCourse> courseToInsert = new List<SemesterCourse>();
            foreach (SemesterCourse oldCourse in semesterCoursesOld)
            {
                bool isDelete = !semesterCoursesNew.Any(criteria => oldCourse.CourseName == criteria.CourseName);
                if (isDelete)
                {
                    courseToDelete.Add(oldCourse);
                }
            }
            foreach (SemesterCourse criteria in courseToDelete)
            {
                DeleteSemesterCourse(connection, criteria);
                semesterCoursesOld.Remove(criteria);
            }


            foreach (SemesterCourse criteria in semesterCoursesNew)
            {
                bool isInsert = !semesterCoursesOld.Any(oldCriteria => oldCriteria.CourseName == criteria.CourseName);
                if (isInsert)
                {
                    courseToInsert.Add(criteria);
                }
            }
            foreach (SemesterCourse criteria in courseToInsert)
            {
                AddSemesterCourse(connection, criteria);
            }
        }

        private static void DeleteSemesterCourse(SqlConnection connection, SemesterCourse course)
        {
            string query = "DELETE FROM semesterCourse WHERE semesterName = @SemesterName AND courseName = @CourseName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SemesterName", course.SemesterName);
                command.Parameters.AddWithValue("@CourseName", course.CourseName);
                command.ExecuteNonQuery();
            }
        }
    }
}
