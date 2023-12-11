namespace SmartUp.DataAccess.SQLServer.Model
{
    public class Course
    {
        public string Name { get; set; }
        public int Credits { get; set; }

        public Course(string name, int credits)
        {
            Name = name;
            Credits = credits;
        }
    }
}
