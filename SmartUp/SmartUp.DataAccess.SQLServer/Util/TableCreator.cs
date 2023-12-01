using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Util;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class TableCreator
    {


        public static void CreateTablesIfNotExist()
        {
            CreateTableIfNotExists("course",
                @"
        IF OBJECT_ID('course', 'U') IS NULL
        BEGIN
            CREATE TABLE course (
                [name] varchar(64),
                credits int NOT NULL,
                PRIMARY KEY([name])
            );
        END;"
            );

            CreateTableIfNotExists("teacher",
                @"
        IF OBJECT_ID('teacher', 'U') IS NULL
        BEGIN
            CREATE TABLE teacher (
                id varchar(32),
                firstName varchar(16) NOT NULL,
                lastName varchar(16) NOT NULL,
                infix varchar(5),
                isMentor bit NOT NULL DEFAULT 0,
                PRIMARY KEY (id)
            );
        END;"
            );

            CreateTableIfNotExists("student",
                @"
        IF OBJECT_ID('student', 'U') IS NULL
        BEGIN
            CREATE TABLE student (
                id varchar(32),
                firstName varchar(16) NOT NULL,
                lastName varchar(16) NOT NULL,
                infix varchar(5),
                mentor varchar(32),
                totalCredits int,
                totalCreditsFromP int,
                class varchar(32),
                PRIMARY KEY (id),
                FOREIGN KEY (mentor) REFERENCES teacher(id)
            );
        END;"
            );

            CreateTableIfNotExists("upcomingExam",
                @"
        IF OBJECT_ID('upcomingExam', 'U') IS NULL
        BEGIN
            CREATE TABLE upcomingExam (
                studentId varchar(32),
                courseName varchar(64),
                [date] DATETIME NOT NULL,
                PRIMARY KEY (studentId, courseName),
                FOREIGN KEY (courseName) REFERENCES course([name])
            );
        END;"
            );

            CreateTableIfNotExists("grade",
                @"
        IF OBJECT_ID('grade', 'U') IS NULL
        BEGIN
            CREATE TABLE grade (
                studentId varchar(32),
                courseName varchar(64),
                attempt int,
                grade decimal(3, 1) NOT NULL,
                isDefinitive bit NOT NULL,
                [date] DATETIME NOT NULL,
                PRIMARY KEY (studentId, courseName, attempt),
                FOREIGN KEY (studentId) REFERENCES student(id)
            );
        END;"
            );

            CreateTableIfNotExists("courseTeacher",
                @"
        IF OBJECT_ID('courseTeacher', 'U') IS NULL
        BEGIN
            CREATE TABLE courseTeacher (
                teacherId varchar(32),
                courseName varchar(64),
                PRIMARY KEY(teacherId, courseName),
                FOREIGN KEY (courseName) REFERENCES course([name]),
                FOREIGN KEY (teacherId) REFERENCES teacher(id)
            );
        END;"
            );

            CreateTableIfNotExists("semester",
                @"
        IF OBJECT_ID('semester', 'U') IS NULL
        BEGIN 
            CREATE TABLE semester (
                [name] varchar(64),
                abbreviation varchar(10),
                [description] TEXT NOT NULL,
                requiredCreditsFromP int DEFAULT 0,
                PRIMARY KEY(abbreviation)
            );
        END;"
            );

            CreateTableIfNotExists("semesterAvailability",
                @"
        IF OBJECT_ID('semesterAvailability', 'U') IS NULL
        BEGIN
            CREATE TABLE semesterAvailability (
                semesterAbbreviation varchar(10),
                availableInSemester int,
                PRIMARY KEY(semesterAbbreviation, availableInSemester),
                FOREIGN KEY (semesterAbbreviation) REFERENCES semester(abbreviation)
            );
        END;"
            );

            CreateTableIfNotExists("semesterCourse",
                @"
        IF OBJECT_ID('semesterCourse', 'U') IS NULL
        BEGIN
            CREATE TABLE semesterCourse (
                semesterAbbreviation varchar(10),
                courseName varchar(64),
                PRIMARY KEY (semesterAbbreviation, courseName),
                FOREIGN KEY (semesterAbbreviation) REFERENCES semester([abbreviation]),
                FOREIGN KEY (courseName) REFERENCES course([name])
            );
        END;"
            );

            CreateTableIfNotExists("semesterCriteria",
                @"
        IF OBJECT_ID('semesterCriteria', 'U') IS NULL
        BEGIN 
            CREATE TABLE semesterCriteria (
                semesterAbbreviation varchar(10),
                courseName varchar(64),
                PRIMARY KEY (semesterAbbreviation, courseName),
                FOREIGN KEY (semesterAbbreviation) REFERENCES semester([abbreviation]),
                FOREIGN KEY (courseName) REFERENCES course([name])
            );
        END;"
            );

            CreateTableIfNotExists("class",
                @"
        IF OBJECT_ID('class', 'U') IS NULL 
        BEGIN
            CREATE TABLE class (
                [name] varchar(32),
                PRIMARY KEY([name])
            );
        END;"
            );
        }

        private static void CreateTableIfNotExists(string tableName, string query)
        {
            using (SqlConnection con = DatabaseConnection.GetConnection())
            {
                try
                {
                    con.Open();
                    string checkTableQuery = $"IF OBJECT_ID('{tableName}', 'U') IS NULL";
                    string createTableQuery = $"{checkTableQuery} BEGIN {query} END;";
                    ExecuteNonQuery(createTableQuery, con);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
                
        }
            
        public static void FillTables()
        {
            CourseDao.GetInstance().FillTable();
            TeacherDao.GetInstance().FillTable();
            SemesterDao.GetInstance().FillTable();
            ClassDao.GetInstance().FillTable();
             StudentDao.GetInstance().FillTable();
             GradeDao.GetInstance().FillTable();
        }

        private static void ExecuteNonQuery(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine($"StackTrace: {ex.StackTrace}");
                }
                
            }
        }
    }
}
