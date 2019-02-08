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
        [Display(Name = "First Course")]
        [DataType(DataType.Date)]
        public DateTime FirstCourse { get; set; }

        [Required]
        [Display(Name = "Day of week")]
        [Range(1, 5, ErrorMessage = "Please choose a day between monday and friday")]
        public DayOfWeek Day { get; set; }

        [Required]
        [Display(Name = "Start hour")]
        [Range(8, 19)]
        public int StartHour { get; set; }

        [Required]
        [Range(0.25, 4)]
        public decimal Duration { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseID { get; set; }

        public virtual Course Course { get; set; }
    }
}