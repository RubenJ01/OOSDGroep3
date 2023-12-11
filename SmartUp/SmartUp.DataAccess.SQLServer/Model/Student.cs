using System.Text;

namespace SmartUp.DataAccess.SQLServer.Model
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Infix { get; set; }
        public string FullName { get; set; }
        public string StudentId { get; set; }

        public Student(string firstName, string lastName, string infix, string studentId)
        {
            FirstName = firstName;
            LastName = lastName;
            Infix = infix;
            StudentId = studentId;

            StringBuilder fullNameBuilder = new StringBuilder();
            fullNameBuilder.Append(firstName);
            if (!string.IsNullOrEmpty(infix))
            {
                fullNameBuilder.Append(" ");
                fullNameBuilder.Append(infix);
            }
            fullNameBuilder.Append(" ");
            fullNameBuilder.Append(lastName);

            FullName = fullNameBuilder.ToString();

        }
    }

}
