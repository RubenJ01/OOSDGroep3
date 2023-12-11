using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Util;
using System.Diagnostics;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class ClassDao
    {
        private static ClassDao? instance;
        private ClassDao()
        {
        }

        public static ClassDao GetInstance()
        {
            if (instance == null)
            {
                instance = new ClassDao();
            }
            return instance;
        }

        public void FillTable()
        {

            using SqlConnection? con = DatabaseConnection.GetConnection();
            try
            {
                con.Open();
                List<String> classNames = GetClassNames();
                foreach (string className in classNames)
                {
                    string insertQuery = "INSERT INTO class (name) VALUES (@name);";
                    using (SqlCommand? command = new SqlCommand(insertQuery, con))
                    {
                        command.Parameters.AddWithValue("@name", className);
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
                    DatabaseConnection.CloseConnection(con);
                }
            }
        }

        public List<String> GetClassNames()
        {
            List<String> semesters = SemesterDao.GetInstance().GetAllSemesterAbbreviations();
            Random? random = new Random();
            List<String> classNames = new List<string>();
            foreach (string semester in semesters)
            {
                int amountOfClasses = random.Next(3) + 1;
                for (int i = 0; i < amountOfClasses; i++)
                {
                    classNames.Add($"{semester}{i + 1}");
                }
            }
            return classNames;
        }
        public List<string> GetClassNameByCourse(string courseName)
        {
            List<string> classNames = new List<string>();
            string query = "SELECT student.class " +
                "FROM student " +
                "JOIN registrationSemester ON registrationSemester.studentId = student.id " +
                "JOIN semesterCourse ON registrationSemester.semesterName = semesterCourse.semesterName " +
                "WHERE  semesterCourse.courseName = @CourseName " +
                "UNION SELECT DISTINCT student.class " +
                "FROM student " +
                "JOIN grade ON grade.studentId = student.id " +
                "JOIN semesterCourse ON grade.courseName = semesterCourse.courseName " +
                "WHERE semesterCourse.courseName = @CourseName; ";
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    if (connection.State != System.Data.ConnectionState.Open) { connection.Open(); };
                    using (SqlCommand? command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CourseName", courseName);
                        using (SqlDataReader? reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string className = reader["Class"].ToString();
                                classNames.Add(className);
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
            return classNames;
        }
    }

}
