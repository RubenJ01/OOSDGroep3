using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUp.DataAccess.SQLServer.Model
{
    public class SemesterRegistration
    {
        public string SemesterAbbreviation {  get; set; }
        public string StudentId { get; set; }

        public SemesterRegistration(string semesterAbbreviation, string studentId)
        {
            this.SemesterAbbreviation = semesterAbbreviation;
            this.StudentId = StudentId;
        }
    }
}
