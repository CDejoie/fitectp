using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniversity.DTO
{
    public class InstructorsDTO
    {
        public int id { get; set; }
        public int CourseId { get; set; }
        public DayOfWeek day { get; set; }
        public int duration { get; set; }
        public List<InstructorsDTO> Schedule { get; set; }
    }
}