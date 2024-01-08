using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class SemesterRequiredPercentageDao
    {
        private static SemesterRequiredPercentageDao instance;

        private SemesterRequiredPercentageDao()
        {
        }

        public static SemesterRequiredPercentageDao GetInstance()
        {
            if (instance == null)
            {
                instance = new SemesterRequiredPercentageDao();
            }
            return instance;
        }

        public List<SemesterRequiredPercentage> GetRequiredPercentages(SqlConnection connection, string semesterName)
        {
            string query = "SELECT requiredSemester, requiredPercentage FROM semesterRequiredPercentage WHERE semesterName = @semesterName";
            List<SemesterRequiredPercentage> percentages = new List<SemesterRequiredPercentage>();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@semesterName", semesterName);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string requiredSemester = reader["requiredSemester"].ToString();
                        int requiredPercentage = Int32.Parse(reader["requiredPercentage"].ToString());  
                        percentages.Add(new SemesterRequiredPercentage(semesterName, requiredSemester, requiredPercentage));
                    }
                }
            }
            return percentages;
        }
    }
}
