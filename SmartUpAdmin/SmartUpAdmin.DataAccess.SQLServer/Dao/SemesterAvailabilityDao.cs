using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using SmartUpAdmin.DataAccess.SQLServer.Model;
using System.Diagnostics;

namespace SmartUpAdmin.DataAccess.SQLServer.Dao
{
    public class SemesterAvailabilityDao
    {
        private static SemesterAvailabilityDao? instance;

        private SemesterAvailabilityDao()
        {
        }

        public static SemesterAvailabilityDao GetInstance()
        {
            if (instance == null)
            {
                instance = new SemesterAvailabilityDao();
            }
            return instance;
        }

        public void AddSemesterAvailability(SqlConnection connection, SemesterAvailability semesterAvailability)
        {
            string query = "INSERT INTO SemesterAvailability (semesterName, availableInSemester) VALUES (@semesterName, @availableInSemester)";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@semesterName", semesterAvailability.SemesterName);
                cmd.Parameters.AddWithValue("@availableInSemester", semesterAvailability.AvailableInSemester);
                cmd.ExecuteNonQuery();
            }

        }

        public List<SemesterAvailability> GetSemesterAvailabiltyBySemester(SqlConnection connection, Semester selectedSemester)
        {
            string query = "SELECT * FROM semesterAvailability WHERE semesterName = @semesterName";
            List<SemesterAvailability> semesterAvailability = new List<SemesterAvailability>();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@semesterName", selectedSemester.Name);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string semesterName = reader["semesterName"].ToString();
                        int availableInSemester = Convert.ToInt32(reader["availableInSemester"]);
                        semesterAvailability.Add(new SemesterAvailability(semesterName, availableInSemester));
                    }
                }
            }
            return semesterAvailability;
        }

        public void UpdateSemesterAvailability(SqlConnection connection, List<SemesterAvailability> semesterAvailabilitiesOld, List<SemesterAvailability> semesterAvailabilitiesNew)
        {
            List<SemesterAvailability> availabilityToDelete = new List<SemesterAvailability>();
            List<SemesterAvailability> availabilityToInsert = new List<SemesterAvailability>();
            foreach (SemesterAvailability oldAvailability in semesterAvailabilitiesOld)
            {
                bool isDelete = !semesterAvailabilitiesNew.Any(availability => oldAvailability.AvailableInSemester == availability.AvailableInSemester);
                if (isDelete)
                {
                    availabilityToDelete.Add(oldAvailability);
                }
            }
            foreach (SemesterAvailability availability in availabilityToDelete)
            {
                DeleteSemesterAvailability(connection, availability);
                semesterAvailabilitiesOld.Remove(availability);
            }


            foreach (SemesterAvailability criteria in semesterAvailabilitiesNew)
            {
                bool isInsert = !semesterAvailabilitiesOld.Any(oldAvailability => oldAvailability.AvailableInSemester == criteria.AvailableInSemester);
                if (isInsert)
                {
                    availabilityToInsert.Add(criteria);
                }
            }
            foreach (SemesterAvailability availability in availabilityToInsert)
            {
                AddSemesterAvailability(connection, availability);
            }
        }

        private void DeleteSemesterAvailability(SqlConnection connection, SemesterAvailability availability)
        {
            string query = "DELETE FROM semesterAvailability WHERE semesterName = @SemesterName AND availableInSemester = @SemesterAvailability";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SemesterName", availability.SemesterName);
                command.Parameters.AddWithValue("@SemesterAvailability", availability.AvailableInSemester);
                command.ExecuteNonQuery();
            }
        }
    }
}
