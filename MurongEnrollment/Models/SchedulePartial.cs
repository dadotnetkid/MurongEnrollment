using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MurongEnrollment.Models
{
    public partial class Schedules
    {
        public string GradeLevelSection { get { return this.Sections?.GradeLevels.GradeLevel + " " + this.Sections?.SectionName; } }
        public string DayTimeRoom { get { return this.Days + " & " + this.Time + " & " + this.Room; } }
        public  string[] Day { get; set; }
        public IEnumerable<object> DaysList
        {
            get
            {
                var d = new List<object>();
                foreach (var i in "Mon,Tue,Wed,Thu,Fri,Sat,Sun".Split(','))
                {
                    var res = new { Day = i };
                    d.Add(res);
                }
                return d;
            }
        }
    }
}