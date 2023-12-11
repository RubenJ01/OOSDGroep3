namespace SmartUpAdmin.DataAccess.SQLServer.Model
{
    public class SemesterCriteria
    {
        public string SemesterName { get; set; }
        public string CourseName { get; set; }

        public SemesterCriteria(string semesterName, string criteriaName)
        {
            SemesterName = semesterName;
            CourseName = criteriaName;
        }
    }
}
