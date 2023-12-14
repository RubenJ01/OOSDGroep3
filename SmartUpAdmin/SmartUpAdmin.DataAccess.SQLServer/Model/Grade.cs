namespace SmartUp.DataAccess.SQLServer.Model
{
    public class Grade
    {
        public decimal GradeNumber { get; set; }
        public bool IsDefinitive { get; set; }
        public DateTime PublishedOn { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }

        public Grade(decimal grade, bool isDefinitive, DateTime publishedOn, string courseName, int credits)
        {
            GradeNumber = grade;
            IsDefinitive = isDefinitive;
            PublishedOn = publishedOn;
            CourseName = courseName;
            Credits = credits;
        }
    }
}
