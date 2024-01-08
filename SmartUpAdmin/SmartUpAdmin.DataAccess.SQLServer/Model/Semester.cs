namespace SmartUp.DataAccess.SQLServer.Model
{
    public class Semester
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }

        public Semester(string name, string abbreviation, string description)
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
        }
    }
}
