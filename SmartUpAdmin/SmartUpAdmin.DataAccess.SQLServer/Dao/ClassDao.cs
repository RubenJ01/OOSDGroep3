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

            using SqlConnection con = DatabaseConnection.GetConnection();
            try
            {
                con.Open();
                List<String> classNames = GetClassNames();
                foreach (string className in classNames)
                {
                    string insertQuery = "INSERT INTO class (name) VALUES (@name);";
                    using (SqlCommand command = new SqlCommand(insertQuery, con))
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
            Random random = new Random();
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
    }
}
