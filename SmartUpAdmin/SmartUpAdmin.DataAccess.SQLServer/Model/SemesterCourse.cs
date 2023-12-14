namespace SmartUp.DataAccess.SQLServer.Model
{
    public class SemesterCourse
    {
        public string SemesterName { get; set; }
        public string CourseName { get; set; }

        public SemesterCourse(string semesterAbbreviation, string courseName)
        {
            SemesterName = semesterAbbreviation;
            CourseName = courseName;
        }
    }
}
