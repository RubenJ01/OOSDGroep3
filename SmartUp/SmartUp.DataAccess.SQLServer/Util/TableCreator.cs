using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Util;
using System.Data;
using System.Diagnostics;

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
                PRIMARY KEY(name)
            );
        END;"
            );

            CreateTableIfNotExists("semesterAvailability",
                @"
        IF OBJECT_ID('semesterAvailability', 'U') IS NULL
        BEGIN
            CREATE TABLE semesterAvailability (
                semesterName varchar(64),
                availableInSemester int,
                PRIMARY KEY(semesterName, availableInSemester),
                FOREIGN KEY (semesterName) REFERENCES semester(name)
            );
        END;"
            );

            CreateTableIfNotExists("semesterCourse",
                @"
        IF OBJECT_ID('semesterCourse', 'U') IS NULL
        BEGIN
            CREATE TABLE semesterCourse (
                semesterName varchar(64),
                courseName varchar(64),
                PRIMARY KEY (semesterName, courseName),
                FOREIGN KEY (semesterName) REFERENCES semester([name]),
                FOREIGN KEY (courseName) REFERENCES course([name])
            );
        END;"
            );

            CreateTableIfNotExists("semesterCriteria",
                @"
        IF OBJECT_ID('semesterCriteria', 'U') IS NULL
        BEGIN 
            CREATE TABLE semesterCriteria (
                semesterName varchar(64),
                courseName varchar(64),
                PRIMARY KEY (semesterName, courseName),
                FOREIGN KEY (semesterName) REFERENCES semester([name]),
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
            CreateTableIfNotExists("registrationSemester",
    @"
        IF OBJECT_ID('registrationSemester', 'U') IS NULL
        BEGIN
            CREATE TABLE registrationSemester (
                studentId varchar(32),
                semesterName varchar(64),
                PRIMARY KEY(studentId, semesterName)
            );
        END;"
            );
            CreateTableIfNotExists("semesterRequiredPercentage",
@"
        IF OBJECT_ID('semesterRequiredPercentage', 'U') IS NULL
        BEGIN
            CREATE TABLE semesterRequiredPercentage (
                semesterName varchar(64),
                requiredSemester varchar(64),
                requiredPercentage int
                PRIMARY KEY(semesterName, requiredSemester)
                FOREIGN KEY (semesterName) REFERENCES semester([name]),
                FOREIGN KEY (requiredSemester) REFERENCES semester([name]),
            );
        END;"
            );
        }

        private static void CreateTableIfNotExists(string tableName, string query)
        {
            using (SqlConnection? con = DatabaseConnection.GetConnection())
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
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        DatabaseConnection.CloseConnection(con);
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
            SemesterCriteriaDao.GetInstance().FillTable();
            SemesterCourseDao.GetInstance().FillTable();
        }

        private static void ExecuteNonQuery(string query, SqlConnection connection)
        {
            using (SqlCommand? command = new SqlCommand(query, connection))
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }

            }
        }


    }
}
