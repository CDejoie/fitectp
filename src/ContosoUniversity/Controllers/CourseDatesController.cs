using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class CourseDatesController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: CourseDates
        public async Task<ActionResult> Index()
        {
            var courseDates = db.CourseDates.Include(c => c.Course);
            return View(await courseDates.ToListAsync());
        }

        // GET: CourseDates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseDate courseDate = await db.CourseDates.FindAsync(id);
            if (courseDate == null)
            {
                return HttpNotFound();
            }
            return View(courseDate);
        }

        // GET: CourseDates/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            return View();
        }

        // POST: CourseDates/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CourseDateID,FirstCourse,Day,StartHour,Duration,CourseID")] CourseDate courseDate)
        {
            if (ModelState.IsValid)
            {
                db.CourseDates.Add(courseDate);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", courseDate.CourseID);
            return View(courseDate);
        }

        // GET: CourseDates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseDate courseDate = await db.CourseDates.FindAsync(id);
            if (courseDate == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", courseDate.CourseID);
            return View(courseDate);
        }

        // POST: CourseDates/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CourseDateID,FirstCourse,Day,StartHour,Duration,CourseID")] CourseDate courseDate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseDate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", courseDate.CourseID);
            return View(courseDate);
        }

        // GET: CourseDates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseDate courseDate = await db.CourseDates.FindAsync(id);
            if (courseDate == null)
            {
                return HttpNotFound();
            }
            return View(courseDate);
        }

        // POST: CourseDates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseDate courseDate = await db.CourseDates.FindAsync(id);
            db.CourseDates.Remove(courseDate);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
