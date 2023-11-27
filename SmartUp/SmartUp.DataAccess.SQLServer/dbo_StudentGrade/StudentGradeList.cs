using Microsoft.Data.SqlClient;
using SmartUp.WPF.Model;
using System.Data.SqlClient;

namespace SmartUp.DataAccess.SQLServer.DBO.StudentGrade
{
    public class StudentGradeList
    {

        public static List<GradeStudentModel> GetStudentGrades(SqlConnection connection, string sql)
        {
            List<GradeStudentModel> grades = new List<GradeStudentModel>();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        decimal grade = reader.GetDecimal(0);
                        bool isDefinitive = reader.GetBoolean(1);
                        DateTime publishedOn = reader.GetDateTime(2);
                        string courseName = reader.GetString(3);
                        int credits = reader.GetInt32(4);
                        GradeStudentModel grademodel = new GradeStudentModel(grade, isDefinitive, publishedOn, courseName, credits);
                        grades.Add(grademodel);
                    }
                }
            }
            return grades;
        }
    }
}
