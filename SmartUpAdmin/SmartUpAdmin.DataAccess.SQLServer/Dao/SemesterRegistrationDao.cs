﻿using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using System.Diagnostics;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class SemesterRegistrationDao
    {
        private static SemesterRegistrationDao? instance;
        private SemesterRegistrationDao()
        {
        }

        public static SemesterRegistrationDao GetInstance()
        {
            if (instance == null)
            {
                instance = new SemesterRegistrationDao();
            }
            return instance;
        }

        public List<SemesterRegistration> GetAllSemesterRegistration()
        {
            string query = "SELECT * FROM registrationSemester";
            List<SemesterRegistration> registrationSemesters = new List<SemesterRegistration>();
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string studentId = reader["studentId"].ToString();
                                string abbreviation = reader["abbreviation"].ToString();
                                registrationSemesters.Add(new SemesterRegistration(abbreviation, studentId));
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
            return registrationSemesters;
        }

        public static void CreateRegistrationByStudentIdBasedOnSemester(string studentId, string abbreviation)
        {
            using SqlConnection con = DatabaseConnection.GetConnection();
            try
            {
                con.Open();

                string query = "INSERT INTO registrationSemester (studentId, abbreviation) " +
                "VALUES (@StudentId, @Abbreviation)";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    command.Parameters.AddWithValue("@Abbreviation", abbreviation);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
            finally
            {
                DatabaseConnection.CloseConnection(con);
            }
        }

        public bool IsEnrolledForSemesterByStudentId(string studentID)
        {
            string query = "SELECT * FROM registrationSemester WHERE studentId = @studentid";
            bool isEnrolled = false;
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentid", studentID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isEnrolled = true;
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
            return isEnrolled;
        }

        public bool IsEnrolledForSemesterByStudentId(string studentID, Semester semester)
        {
            string query = "SELECT * FROM registrationSemester WHERE studentId = @studentid AND abbreviation = @semesterAbbreviation";
            bool isEnrolled = false;
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentid", studentID);
                        command.Parameters.AddWithValue("@semesterAbbreviation", semester.Abbreviation);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isEnrolled = true;
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
            return isEnrolled;
        }

        public static void UnsubscribeFromSemesterByStudentId(string studentID)
        {
            string query = "DELETE FROM registrationSemester WHERE studentId = @studentid";
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentid", studentID);
                        command.ExecuteNonQuery();
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
        }

    }
}
