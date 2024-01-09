using SmartUp.DataAccess.SQLServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUpAdmin.DataAccess.SQLServer.Model
{
    public class SemesterRequiredPercentage
    {
        public string SemesterName { get; set; }
        public string RequiredSemesterName { get; set; }
        public int RequiredPercentage { get; set; }

        public SemesterRequiredPercentage(string semesterName, string requiredSemesterName, int requiredPercentage)
        {
            SemesterName = semesterName;
            RequiredSemesterName = requiredSemesterName;
            RequiredPercentage = requiredPercentage;
        }

        public SemesterRequiredPercentage()
        {
        }
    }
}
