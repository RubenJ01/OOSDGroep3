using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUp.DataAccess.SQLServer.Model
{
    public class GradeTeacher
    {
        //In Dutch, because of the propertynames are tablenames
        public string StudentId { get; set; }
        public string Naam { get; set; }
        public string Vak { get; set; }
        public decimal? Cijfer { get; set; }
        public bool? IsDefinitief { get; set; }



        public GradeTeacher(decimal grade, bool isDefinitive, string courseName, Student student)
        {
            Cijfer = grade;
            IsDefinitief = isDefinitive;
            Vak = courseName;
            StudentId = student.StudentId;
            Naam = student.FullName;
        }
        public GradeTeacher(string courseName, Student student)
        {
            Cijfer = null;
            IsDefinitief = null;
            Vak = courseName;
            StudentId = student.StudentId;
            Naam = student.FullName;
        }
    }
}
