namespace SmartUp.DataAccess.SQLServer.Model
{
    public class SemesterCourse
    {
        public string SemesterName { get; set; }
        public string CourseName { get; set; }

        public SemesterCourse(string semesterName, string courseName)
        {
            SemesterName = semesterName;
            CourseName = courseName;
        }
    }
}
