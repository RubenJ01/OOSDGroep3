using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUp.DataAccess.SQLServer.Model
{
    public class SemesterCourse
    {
        public string SemesterName {  get; set; }
        public string CourseName { get; set; }

        public SemesterCourse(string semesterAbbreviation, string courseName)
        {
            SemesterName = semesterAbbreviation;
            CourseName = courseName;
        }
    }
}
