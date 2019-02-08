using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniversity.DTO
{
    public class StudentsDTO
    {
        public int id { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string enrollmentDate { get; set; }
        public List<Dictionary<string, string>> enrollments { get; set; }
    }
}