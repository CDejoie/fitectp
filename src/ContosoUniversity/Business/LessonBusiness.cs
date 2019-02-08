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
        public CourseDate FindLesson (int? lessonId)
        {
            return db.CourseDates.Find(lessonId);
        }
        public void RemoveLesson(CourseDate lesson)
        {
            db.CourseDates.Remove(lesson);
            db.SaveChanges();
        }
        public DateTime FirstCourseDate (int courseId)
        {
            try
            {
                CourseDate course = db.CourseDates.FirstOrDefault(c => c.CourseID == courseId);
                return course.FirstCourse;
            }
            catch (Exception)
            {
                return default(DateTime);
            }
        }
        public void SaveDataBase()
        {
            db.SaveChanges();
        }
    }
}