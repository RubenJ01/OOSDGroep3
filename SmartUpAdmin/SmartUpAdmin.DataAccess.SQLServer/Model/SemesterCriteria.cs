using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUpAdmin.DataAccess.SQLServer.Model
{
    public class SemesterCriteria
    {
        public string SemesterName { get; set; }
        public string CourseName { get; set; }

        public SemesterCriteria(string semesterName, string criteriaName)
        {
            SemesterName = semesterName;
            CourseName = criteriaName;
        }
    }
}
