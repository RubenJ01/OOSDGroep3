using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUp.WPF.Model
{
    public class StudentGradeModel
    {
        public List<GradeModel> Grades { get; set; }
        public List<GradeModel> GetGrades()
        {
            //Grades = SmartUp.DataAccess.SQLServer.GetGradesFromDatabase();
            return Grades;
        }
    }
}
