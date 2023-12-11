namespace SmartUp.DataAccess.SQLServer.Model
{
    public class GradeTeacher
    {
        public string StudentId { get; set; }
        public string Naam { get; set; }
        public string Vak { get; set; }
        public decimal? Cijfer { get; set; }
        public string? Status { get; set; }



        public GradeTeacher(decimal grade, string status, string courseName, Student student)
        {
            Cijfer = grade;
            Status = status;
            Vak = courseName;
            StudentId = student.StudentId;
            Naam = student.FullName;
        }
        public GradeTeacher(string courseName, Student student)
        {
            Cijfer = null;
            Status = null;
            Vak = courseName;
            StudentId = student.StudentId;
            Naam = student.FullName;
        }
    }
}
