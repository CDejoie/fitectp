using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContosoUniversity.Models
{
    public class CourseDate
    {
     
        public int CourseDateID { get; set; }
        public DateTime FirstCourse { get; set; }
        public DayOfWeek Day { get; set; }
        public string StartHour { get; set; }
        public int Duration { get; set; }
        public int CourseID { get; set; }

        public virtual Course Course { get; set; }
    }
}