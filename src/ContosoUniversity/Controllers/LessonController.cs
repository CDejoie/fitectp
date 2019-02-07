using ContosoUniversity.Business;
using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContosoUniversity.Controllers
{
    public class LessonController : Controller
    {
        LessonBusiness lessonB = new LessonBusiness();

        private void PopulateCourseDropDownList(object selectedCourse = null)
        {
            List<Course> courseQuery = lessonB.CourseList();
            ViewBag.CourseID = new SelectList(courseQuery, "CourseID", "Title", selectedCourse);
        }

        // GET: Lesson
        public ActionResult Index(int? SelectedCourse)
        {
            List<Course> courses = lessonB.CourseList();
            ViewBag.SelectedCourse = new SelectList(courses, "CourseID", "Title", SelectedCourse);
            int courseID = SelectedCourse.GetValueOrDefault();

            return View(lessonB.ListeFiltreLesson(SelectedCourse, courseID).ToList());
        }

        public ActionResult Create()
        {
            PopulateCourseDropDownList();
            return View();
        }

        // POST: Lesson
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseDate lesson)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    lessonB.AddLesson(lesson);

                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            PopulateCourseDropDownList(lesson.CourseDateID);

            return View(lesson);
        }
    }
}