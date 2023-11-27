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
                for (int i = 0; i < 20; i++)
                {
                    string firstName = NameGenerator.GenerateRandomName();
                    string infix = "";
                    if (random.Next(5) == 3)
                    {
                        infix = NameGenerator.GenerateRandomInfix();
                    }
                    string lastName = NameGenerator.GenerateRandomName();
                    string studentId = $"S{i.ToString($"D{6}")}";

                    // Assuming you have a teachers table with columns: teacherId, firstName, infix, lastName, isMentor
                    string insertQuery = "INSERT INTO student (id, firstName, infix, lastName, mentor, totalCredits, totalCreditsFromP, class) " +
                                         "VALUES (@TeacherId, @FirstName, @Infix, @LastName, @IsMentor);";

                    using (SqlCommand command = new SqlCommand(insertQuery, con))
                    {
                        // command.Parameters.AddWithValue("@TeacherId", teacherId);

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


    }
}
