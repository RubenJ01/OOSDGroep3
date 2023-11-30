using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUp.DataAccess.SQLServer.Model
{
    public class SemesterCourse
    {
        public string SemesterAbbreviation {  get; set; }
        public string CourseName { get; set; }

        public SemesterCourse(string semesterAbbreviation, string courseName)
        {
            SemesterAbbreviation = semesterAbbreviation;
            CourseName = courseName;
        }
    }
}
