using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContosoUniversity.Business
{
    public class LessonBusiness
    {
        private SchoolContext db;
        public LessonBusiness()
        {
            db = new SchoolContext();
        }

        public List<Course> CourseList()
        {
            return db.Courses.OrderBy(c => c.Title).ToList();
        }

        public IQueryable<CourseDate> ListeFiltreLesson(int? selectedCourse, int courseId)
        {
            return db.CourseDates
                .Where(c => !selectedCourse.HasValue || c.CourseID == courseId)
                .OrderBy(c => c.CourseDateID);
        }

        public void AddLesson(CourseDate lesson)
        {
            db.CourseDates.Add(lesson);
            db.SaveChanges();
        }

    }
}