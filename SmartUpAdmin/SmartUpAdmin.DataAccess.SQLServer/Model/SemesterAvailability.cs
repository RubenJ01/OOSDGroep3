namespace SmartUpAdmin.DataAccess.SQLServer.Model
{
    public class SemesterAvailability
    {
        public string SemesterName { get; set; }
        public int AvailableInSemester { get; set; }

        public SemesterAvailability(string semesterName, int availableInSemester)
        {
            this.SemesterName = semesterName;
            this.AvailableInSemester = availableInSemester;
        }
    }
}