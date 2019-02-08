using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContosoUniversity.DTO
{
    public class StudentsDTO
    {

        public int id { get; set; }
        public string lastName { get; set; }
        public string firstMidName { get; set; }
        public DateTime enrollmentDate { get; set; }
        public List<Dictionary<string, string>> enrollments { get; set; }


    }
}
