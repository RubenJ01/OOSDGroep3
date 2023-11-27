using SmartUp.Core.Util;
using SmartUp.DataAccess.SQLServer.Util;
using System.Data.SqlClient;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class TeacherDao
    {
        private static TeacherDao? instance;
        private TeacherDao()
        {
        }

        public static TeacherDao GetInstance()
        {
            if (instance == null)
            {
                instance = new TeacherDao();
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
                    bool isMentor = false;
                    if (random.Next(5) == 3)
                    {
                        infix = NameGenerator.GenerateRandomInfix();
                        isMentor = true;
                    }
                    string lastName = NameGenerator.GenerateRandomName();
                    string teacherId = $"T{i.ToString($"D{6}")}";

                    // Assuming you have a teachers table with columns: teacherId, firstName, infix, lastName, isMentor
                    string insertQuery = "INSERT INTO teacher (id, firstName, infix, lastName, isMentor) " +
                                         "VALUES (@TeacherId, @FirstName, @Infix, @LastName, @IsMentor);";

                    using (SqlCommand command = new SqlCommand(insertQuery, con))
                    {
                        command.Parameters.AddWithValue("@TeacherId", teacherId);
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@Infix", infix);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@IsMentor", isMentor);

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
