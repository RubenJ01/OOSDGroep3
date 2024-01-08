using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using SmartUpAdmin.DataAccess.SQLServer.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUpAdmin.DataAccess.SQLServer.Dao
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

        public void AddSemesterRequiredPercentage(SqlConnection connection, SemesterRequiredPercentage semesterRequiredPercentage)
        {
            string query = "INSERT INTO semesterRequiredPercentage (semesterName, requiredSemester, requiredPercentage) " +
                "VALUES(@semesterName, @requiredSemester, @requiredPercentage)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@semesterName", semesterRequiredPercentage.SemesterName);
                command.Parameters.AddWithValue("@requiredSemester", semesterRequiredPercentage.RequiredSemesterName);
                command.Parameters.AddWithValue("@requiredPercentage", semesterRequiredPercentage.RequiredPercentage);
                command.ExecuteNonQuery();
            }
        }

        public List<SemesterRequiredPercentage> GetAllSemesterRequiredPercentage(SqlConnection connection)
        {
            string query = "SELECT * FROM semesterRequiredPercentage";
            List<SemesterRequiredPercentage> semesterRequiredPercentage = new List<SemesterRequiredPercentage>();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string semesterName = reader["semesterName"].ToString();
                        string requiredSemester = reader["requiredSemester"].ToString();
                        int requiredPercentage = Convert.ToInt32(reader["requiredPercentage"]);
                        semesterRequiredPercentage.Add(new SemesterRequiredPercentage(semesterName, requiredSemester, requiredPercentage));
                    }
                }
            }
            return semesterRequiredPercentage;
        }

        public List<SemesterRequiredPercentage> GetAllSemesterRequiredPercentageBySemester(SqlConnection connection, Semester semester)
        {
            string query = "SELECT * FROM semesterRequiredPercentage WHERE semesterName = @semesterName";
            List<SemesterRequiredPercentage> semesterRequiredPercentage = new List<SemesterRequiredPercentage>();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@semesterName", semester.Name);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string semesterName = reader["semesterName"].ToString();
                        string requiredSemester = reader["requiredSemester"].ToString();
                        int requiredPercentage = Convert.ToInt32(reader["requiredPercentage"]);
                        semesterRequiredPercentage.Add(new SemesterRequiredPercentage(semesterName, requiredSemester, requiredPercentage));
                    }
                }
            }
            return semesterRequiredPercentage;
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

        public void UpdateSemesterRequiredPercentage(SqlConnection connection, List<SemesterRequiredPercentage> requiredPercentagesOld, List<SemesterRequiredPercentage> semesterRequiredPercentagesNew)
        {
            Debug.WriteLine("Entering UpdateSemesterRequiredPercentage");

            List<SemesterRequiredPercentage> percentageToDeleteOrUpdate = new List<SemesterRequiredPercentage>();
            List<SemesterRequiredPercentage> percentageToInsert = new List<SemesterRequiredPercentage>();

            requiredPercentagesOld.ForEach(requiredPercentagesOld => Debug.WriteLine($"{requiredPercentagesOld.RequiredSemesterName}, {requiredPercentagesOld.RequiredPercentage}  -> {requiredPercentagesOld.SemesterName}"));
            semesterRequiredPercentagesNew.ForEach(requiredPercentagesOld => Debug.WriteLine($"{requiredPercentagesOld.RequiredSemesterName}, {requiredPercentagesOld.RequiredPercentage} -> {requiredPercentagesOld.SemesterName}"));

            foreach (SemesterRequiredPercentage oldPercentage in requiredPercentagesOld)
            {
                bool isDeleteOrUpdate = !semesterRequiredPercentagesNew.Any(percentage => oldPercentage.RequiredSemesterName == percentage.RequiredSemesterName && oldPercentage.RequiredPercentage == percentage.RequiredPercentage);

                if (isDeleteOrUpdate)
                {
                    percentageToDeleteOrUpdate.Add(oldPercentage);
                    Debug.WriteLine($"Marked for delete or update: {oldPercentage.RequiredSemesterName}, {oldPercentage.RequiredPercentage}");
                }
            }


            foreach (SemesterRequiredPercentage percentageToDeleteUpdate in percentageToDeleteOrUpdate)
            {
                Debug.WriteLine($"Processing delete or update: {percentageToDeleteUpdate.RequiredSemesterName}, {percentageToDeleteUpdate.RequiredPercentage}");

                SemesterRequiredPercentage semesterRequiredPercentageNew = semesterRequiredPercentagesNew.FirstOrDefault(percentage => percentageToDeleteUpdate.RequiredSemesterName == percentage.RequiredSemesterName);

                if (semesterRequiredPercentageNew != null && semesterRequiredPercentageNew.RequiredPercentage != percentageToDeleteUpdate.RequiredPercentage)
                {
                    Debug.WriteLine($"Updating: {semesterRequiredPercentageNew.RequiredSemesterName}, {semesterRequiredPercentageNew.RequiredPercentage}");

                    string query = "UPDATE semesterRequiredPercentage SET requiredPercentage = @requiredPercentage WHERE semesterName = @semesterName AND requiredSemester = @requiredSemester";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@semesterName", semesterRequiredPercentageNew.SemesterName);
                        command.Parameters.AddWithValue("@requiredSemester", semesterRequiredPercentageNew.RequiredSemesterName);
                        command.Parameters.AddWithValue("@requiredPercentage", semesterRequiredPercentageNew.RequiredPercentage);

                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    Debug.WriteLine($"Deleting: {percentageToDeleteUpdate.RequiredSemesterName}, {percentageToDeleteUpdate.RequiredPercentage}");

                    DeleteSemesterRequiredPercentage(connection, percentageToDeleteUpdate);
                    requiredPercentagesOld.Remove(percentageToDeleteUpdate);
                }
            }



            foreach (SemesterRequiredPercentage percentage in semesterRequiredPercentagesNew)
            {
                Debug.WriteLine($"Processing insert: {percentage.RequiredSemesterName}, {percentage.RequiredPercentage}");

                bool isInsert = !requiredPercentagesOld.Any(oldPercentage => oldPercentage.RequiredPercentage == percentage.RequiredPercentage && oldPercentage.RequiredSemesterName == percentage.RequiredSemesterName);

                if (isInsert)
                {
                    Debug.WriteLine($"Inserting: {percentage.RequiredSemesterName}, {percentage.RequiredPercentage}");

                    percentageToInsert.Add(percentage);
                }
            }

            foreach (SemesterRequiredPercentage percentage in percentageToInsert)
            {
                Debug.WriteLine($"Adding: {percentage.RequiredSemesterName}, {percentage.RequiredPercentage}");

                AddSemesterRequiredPercentage(connection, percentage);
            }

            Debug.WriteLine("Exiting UpdateSemesterRequiredPercentage");
        }

        private void DeleteSemesterRequiredPercentage(SqlConnection connection, SemesterRequiredPercentage semesterRequiredPercentage)
        {
            string query = "DELETE FROM semesterRequiredPercentage WHERE semesterName = @semesterName AND requiredSemester = @requiredSemester AND requiredPercentage = @requiredPercentage";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                Debug.WriteLine(8);
                command.Parameters.AddWithValue("@semesterName", semesterRequiredPercentage.SemesterName);
                command.Parameters.AddWithValue("@requiredSemester", semesterRequiredPercentage.RequiredSemesterName);
                command.Parameters.AddWithValue("@requiredPercentage", semesterRequiredPercentage.RequiredPercentage);
                command.ExecuteNonQuery();
            }
        }
    }
}
