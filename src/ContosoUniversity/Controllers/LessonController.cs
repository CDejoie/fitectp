using ContosoUniversity.Business;
using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
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
            DateTime firstCourse = lessonB.FirstCourseDate(lesson.CourseID);

            try
            {
                if (ModelState.IsValid && (firstCourse == default(DateTime) || firstCourse == lesson.FirstCourse))
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

            if (firstCourse != default(DateTime) && firstCourse != lesson.FirstCourse)
            {
                ModelState.AddModelError("", "This course already have first date course : " + firstCourse.ToString("d"));

                PopulateCourseDropDownList(lesson.CourseDateID);

                return View(lesson);
            }

            PopulateCourseDropDownList(lesson.CourseDateID);

            return View(lesson);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CourseDate lesson = lessonB.FindLesson(id);

            if (lesson == null)
            {
                return HttpNotFound();
            }

            PopulateCourseDropDownList(lesson.CourseDateID);
            return View(lesson);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CourseDate lessonToUpdate = lessonB.FindLesson(id);
            DateTime firstCourse = lessonB.FirstCourseDate(lessonToUpdate.CourseID);

            if (TryUpdateModel(lessonToUpdate, "", new string[] { "FirstCourse", "Day", "StartHour", "Duration" }) && (firstCourse == default(DateTime) || firstCourse == lessonToUpdate.FirstCourse))
            {
                try
                {
                    lessonB.SaveDataBase();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            if (firstCourse != default(DateTime) && firstCourse != lessonToUpdate.FirstCourse)
            {
                ModelState.AddModelError("", "This course already have first date course : " + firstCourse.ToString("d"));

                PopulateCourseDropDownList(lessonToUpdate.CourseDateID);

                return View(lessonToUpdate);
            }

            PopulateCourseDropDownList(lessonToUpdate.CourseDateID);
            return View(lessonToUpdate);
        }

        // GET: Lesson/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CourseDate lesson = lessonB.FindLesson(id);

            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseDate lesson = lessonB.FindLesson(id);
            lessonB.RemoveLesson(lesson);

            return RedirectToAction("Index");
        }
    }
}