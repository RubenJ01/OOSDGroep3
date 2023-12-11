namespace SmartUp.DataAccess.SQLServer.Model
{
    public class GradeStudent
    {
        public decimal? GradeNumber { get; set; }
        public bool? IsDefinitive { get; set; }
        public DateTime PublishedOn { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }
        public Student Student { get; set; }


        public GradeStudent(decimal grade, bool isDefinitive, DateTime publishedOn, string courseName, int credits)
        {
            GradeNumber = grade;
            IsDefinitive = isDefinitive;
            PublishedOn = publishedOn;
            CourseName = courseName;
            Credits = credits;
        }
        public GradeStudent(decimal grade, bool isDefinitive, string courseName, Student student)
        {
            GradeNumber = grade;
            IsDefinitive = isDefinitive;
            CourseName = courseName;
            this.Student = student;
        }
        public GradeStudent(string courseName, Student student)
        {
            CourseName = courseName;
            this.Student = student;
            IsDefinitive = null;
            GradeNumber = null;
        }
    }
}
