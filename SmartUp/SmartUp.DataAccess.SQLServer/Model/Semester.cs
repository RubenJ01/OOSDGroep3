using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUp.DataAccess.SQLServer.Model
{
    public class Semester
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public int RequiredCreditsFromP {  get; set; }

        public Semester(string name, string abbreviation, string description, int creditsFromP)
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            RequiredCreditsFromP = creditsFromP;
        }
    }
}
