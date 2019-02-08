using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.AuthData;

namespace ContosoUniversity.Controllers
{
    [AuthenticationFilter]
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        public SchoolContext DbContext

        {
            get { return db; }
            set { db = value; }
        }

        // GET: Student
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var students = from s in db.Students
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:  // Name ascending 
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }


        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            //Ajout de TempData afin de transférer l'id de l'étudiant avec les controllers Subscribe et Subscribtion
            TempData["StudentID"] = id;

            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName, FirstMidName, EnrollmentDate")]Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(student);
        }


        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentToUpdate = db.Students.Find(id);
            if (TryUpdateModel(studentToUpdate, "",
               new string[] { "LastName", "FirstMidName", "EnrollmentDate" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                db.Students.Remove(student);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        // GET: Student/Subscribe
        public ActionResult Subscribe()
        {
            //On récupère la liste de tous les cours pour l'envoyer dans la vue
            List<Course> Courses = db.Courses.ToList();
            TempData["StudentID"] = TempData["StudentID"];

            return View(Courses);
        }

        public ActionResult Subscribtion(int id)
        {
            int studentID = (int)TempData["StudentID"];
            Enrollment enrollementFind = db.Enrollments.FirstOrDefault(e => e.StudentID == studentID && e.CourseID == id);

            if (enrollementFind == null)
            {
                db.Enrollments.Add(new Enrollment { CourseID = id, StudentID = studentID });
                db.SaveChanges();
            }
            else
            {
                ViewBag.ErrorMessage = "You already subscribed to this lesson";

                List<Course> Courses = db.Courses.ToList();
                TempData["StudentID"] = TempData["StudentID"];

                return View("Subscribe", Courses);
            }

            return RedirectToAction("Details", new { controller = "Student", action = "Details", id = studentID });
        }

        //GET add Student profile picture
        [HttpGet]
        public ActionResult ProfilPicture(int id)
        {
            Student studentFind = db.Students.First(s => s.ID == id);
            TempData["StudentID"] = id;

            return View(studentFind);
        }

        //POST add Student profile picture
        [HttpPost]
        public ActionResult ProfilPicture(HttpPostedFileBase file)
        {
            int studentID = (int)TempData["StudentID"];
            Student studentFind = db.Students.First(s => s.ID == studentID);

            if (file == null)
            {
                ViewBag.Message = "File doesn't exist";
                TempData["StudentID"] = TempData["StudentID"];

                return View();
            }
            else if (file.ContentLength > 100000)
            {
                ViewBag.Message = "File exceed 100KB";
                TempData["StudentID"] = TempData["StudentID"];

                return View();
            }
            // On test si le fichier n'est pas un fichier en extension ".jpeg" (avec la classe MediaTypeNames) ou ".png" (pas d'equivalent dans la classe MediaTypeNames)
            else if (file.ContentType != MediaTypeNames.Image.Jpeg && file.ContentType != "image/png")
            {
                ViewBag.Message = "Bad file extension";
                TempData["StudentID"] = TempData["StudentID"];
                return View();
            }
            else
            {
                string filepath = Path.Combine(Server.MapPath("~/ProfilPictures"), studentFind.FirstMidName + studentFind.LastName + ".jpeg");
                file.SaveAs(filepath);

                studentFind.ProfilePictureLink = "/ProfilPictures/" + studentFind.FirstMidName + studentFind.LastName + ".jpeg";
                db.SaveChanges();

                return View("Details", studentFind);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
