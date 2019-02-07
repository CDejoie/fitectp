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

        [Required]
        [DataType(DataType.Date)]
        public DateTime FirstCourse { get; set; }

        [Required]
        [Range(1, 5)]
        public DayOfWeek Day { get; set; }

        [Required]
        [Range(8, 19)]
        public int StartHour { get; set; }

        [Required]
        [Range(0.25, 4)]
        public decimal Duration { get; set; }

        [Required]
        public int CourseID { get; set; }

        public virtual Course Course { get; set; }
    }
}