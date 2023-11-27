using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUp.WPF.Model
{
    public class GradeStudentModel
    {
        public decimal Grade { get; set; }
        public bool IsDefinitive { get; set; }
        public DateTime PublishedOn { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }
        
        public GradeStudentModel(decimal grade, bool isDefinitive, DateTime publishedOn, string courseName, int credits) 
        {
            Grade = grade;
            IsDefinitive = isDefinitive;
            PublishedOn = publishedOn;
            CourseName = courseName;
            Credits = credits;
        }
    }
}
