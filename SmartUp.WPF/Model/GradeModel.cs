using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUp.WPF.Model
{
    public class GradeModel
    {
        public double Grade { get; set; }
        public bool IsDefinitive { get; set; }
        public DateTime PublishedOn { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }
        
        public GradeModel(double grade, bool isDefinitive, DateTime publishedOn, string courseName, int credits) 
        {
            Grade = grade;
            IsDefinitive = isDefinitive;
            PublishedOn = publishedOn;
            CourseName = courseName;
            Credits = credits;
        }
    }
}
