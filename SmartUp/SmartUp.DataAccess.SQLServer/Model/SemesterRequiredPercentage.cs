using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUp.DataAccess.SQLServer.Model
{
    public class SemesterRequiredPercentage
    {
        public string SemesterName {  get; set; }
        public string RequiredSemester {  get; set; }
        public int RequiredPercentage { get; set; }

        public SemesterRequiredPercentage(string semesterName, string requiredSemester, int requiredPercentage) 
        {
            SemesterName = semesterName;
            RequiredSemester = requiredSemester;
            RequiredPercentage = requiredPercentage;
        }
    }
}
