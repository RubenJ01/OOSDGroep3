namespace SmartUp.DataAccess.SQLServer.Model
{
    public class SemesterRegistration
    {
        public string SemesterAbbreviation { get; set; }
        public string StudentId { get; set; }

        public SemesterRegistration(string semesterAbbreviation, string studentId)
        {
            this.SemesterAbbreviation = semesterAbbreviation;
            this.StudentId = StudentId;
        }
    }
}
