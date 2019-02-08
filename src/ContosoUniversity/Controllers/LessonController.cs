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

        //Methode permettant de créer la liste des cours sur lesquels on filtre sur la vue Index
        private void PopulateCourseDropDownList(object selectedCourse = null)
        {
            //Initialise la liste
            List<Course> courseQuery = lessonB.CourseList();
            //On passe dans un Viewbag notre liste de course avec une mise en forme pouvant être exploité par un DropDownList
            ViewBag.CourseID = new SelectList(courseQuery, "CourseID", "Title", selectedCourse);
        }

        // GET: Lesson
        public ActionResult Index(int? SelectedCourse)
        {
            //Récupère la liste des cours
            List<Course> courses = lessonB.CourseList();

            // On passe dans un Viewbag notre liste de course avec une mise en forme pouvant être exploité par un DropDownList
            ViewBag.SelectedCourse = new SelectList(courses, "CourseID", "Title", SelectedCourse);

            //On récupère le cours qui a été selectionné
            int courseID = SelectedCourse.GetValueOrDefault();

            //Retourne la vue index avec le filtre s'il existe
            return View(lessonB.ListeFiltreLesson(SelectedCourse, courseID).ToList());
        }

        //GET : Create Lesson
        public ActionResult Create()
        {
            //Transfère a la vue la liste des cours auquels on peut s'inscrire
            PopulateCourseDropDownList();
            return View();
        }

        // POST: Create Lesson
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseDate lesson)
        {
            //On récupère la date du premier cours (si jamais il a été défini auparavant par une lesson)
            DateTime firstCourse = lessonB.FirstCourseDate(lesson.CourseID);

            try
            {
                //Si le lesson valide les conditions du model et soit il n'y a pas déjà eu de lesson de défini pour le cours 
                //soit la date du premier cour est identique à celle de la/les lesson(s) déjà enregistrés
                if (ModelState.IsValid && (firstCourse == default(DateTime) || firstCourse == lesson.FirstCourse))
                {
                    //On enregistre la lesson dans la base de données
                    lessonB.AddLesson(lesson);

                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            //Si une lesson a déjà été enregistré et que la date du premier cours est different de celle renseigné, on renvoie un message d'erreur
            if (firstCourse != default(DateTime) && firstCourse != lesson.FirstCourse)
            {
                ModelState.AddModelError("", "This course already have first date course : " + firstCourse.ToString("d"));

                PopulateCourseDropDownList(lesson.CourseDateID);

                return View(lesson);
            }

            PopulateCourseDropDownList(lesson.CourseDateID);

            return View(lesson);
        }

        //GET Edit Lesson
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //On recherche la lesson corespondant à l'id
            CourseDate lesson = lessonB.FindLesson(id);

            //Si la lesson est nul, page d'erreur
            if (lesson == null)
            {
                return HttpNotFound();
            }

            PopulateCourseDropDownList(lesson.CourseDateID);
            return View(lesson);
        }

        //POST Edit Lesson
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Recherche la lesson corespondant à l'id
            CourseDate lessonToUpdate = lessonB.FindLesson(id);

            //On récupère la date du premier cours (si jamais il a été défini auparavant par une lesson)
            DateTime firstCourse = lessonB.FirstCourseDate(lessonToUpdate.CourseID);

            //On essaye de mettre à jour la lesson, il faut que soit la date du premier cours n'existe pas, soit la date du premier cours soit égale à celle defini
            if (TryUpdateModel(lessonToUpdate, "", new string[] { "FirstCourse", "Day", "StartHour", "Duration" }) && (firstCourse == default(DateTime) || firstCourse == lessonToUpdate.FirstCourse))
            {
                try
                {
                    //Dans ce cas, on enregistre
                    lessonB.SaveDataBase();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            //Si la date du premier cours existe et est différente de celle definie, on renvoie une erreur
            if (firstCourse != default(DateTime) && firstCourse != lessonToUpdate.FirstCourse)
            {
                ModelState.AddModelError("", "This course already have first date course : " + firstCourse.ToString("d"));

                PopulateCourseDropDownList(lessonToUpdate.CourseDateID);

                return View(lessonToUpdate);
            }

            PopulateCourseDropDownList(lessonToUpdate.CourseDateID);
            return View(lessonToUpdate);
        }

        // GET: Lesson/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //On recherche la lesson corespondant à l'id
            CourseDate lesson = lessonB.FindLesson(id);

            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // POST: Course/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //On recherche la lesson corespondant à l'id
            CourseDate lesson = lessonB.FindLesson(id);
            //On supprime la lesson
            lessonB.RemoveLesson(lesson);

            return RedirectToAction("Index");
        }
    }
}