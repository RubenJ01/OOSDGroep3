using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUp.DataAccess.SQLServer.Model
{
    public class SbStudentModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Infix { get; set; }
        public string FullName { get; set; }
        public string StudentId { get; set; }

        public SbStudentModel(string firstName, string lastName, string infix, string studentId)
        {
            FirstName = firstName;
            LastName = lastName;
            Infix = infix;
            StudentId = studentId;
            if (string.IsNullOrEmpty(infix))
            {
                FullName = firstName + " " + lastName;
            }
            else
            {
                FullName = firstName + " " + infix + " " + lastName;
            }
        }
    }

}
