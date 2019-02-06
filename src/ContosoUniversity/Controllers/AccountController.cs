using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ContosoUniversity.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            using (SchoolContext db = new SchoolContext())
            {
                return View(db.People.ToList());
            }
        }
        public ActionResult Register()
        {
            List<string> typePerson = new List<string>();
            typePerson.Add("Student");
            typePerson.Add("Instructor");

            ViewBag.typePerson = new SelectList(typePerson);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string ChoixPerson, string LastName, string FirstMidName, string Email, string UserName, string Password, string ConfirmPassword)
        {
            SchoolContext db = new SchoolContext();

            if (ModelState.IsValid)
            {
                if (db.People.Any(x => x.UserName == UserName))
                {
                    ModelState.AddModelError("User Name", "User Name already exists.");
                    ViewBag.MessageDoublon = "The login " + UserName + " already exists. Try again";
                }
                else
                {
                    if (ChoixPerson == "Student")
                    {
                        AccountBusiness.RegistredConfirmationStudent(ChoixPerson, LastName, FirstMidName, Email, UserName, Password, ConfirmPassword);
                        ModelState.Clear();
                        ViewBag.Message =" " + FirstMidName + " " + LastName + " successfully registred.";
                    }
                    else
                    {
                        AccountBusiness.RegistredConfirmationInstructor(ChoixPerson, LastName, FirstMidName, Email, UserName, Password, ConfirmPassword);
                        ModelState.Clear();
                        ViewBag.Message = " " +FirstMidName + " " + LastName + " successfully registred.";
                    }
                }

            }

            List<string> typePerson = new List<string>();
            typePerson.Add("Student");
            typePerson.Add("Instructor");
            ViewBag.typePerson = new SelectList(typePerson);

            return View();
        }
      

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Student student, Instructor instructor, string returnUrl)
        {
            SchoolContext db = new SchoolContext();
            ViewBag.ReturnUrl = returnUrl;
            if (student.EnrollmentDate != null)
            {

                var user = db.People.SingleOrDefault(x => x.UserName == student.UserName && x.Password == student.Password);
                if (user != null)
                {
                    Session["ID"] = user.ID.ToString();
                    Session["UserName"] = user.UserName.ToString();
                    return RedirectToAction("Index", "Home");
                }
                if (user == null)
                {
                    {
                        ModelState.AddModelError(string.Empty, "Username or Password is wrong");
                        return View(student);
                    }
                }

                // Par défaut, rediriger vers la page d'accueil :
                return RedirectToAction("Index", "Home");

            }
            else
            {

                var user = db.People.SingleOrDefault(x => x.UserName == instructor.UserName && x.Password == instructor.Password);
                if (user != null)
                {
                    Session["ID"] = user.ID.ToString();
                    Session["UserName"] = user.UserName.ToString();
                    return RedirectToAction("Index", "Home");
                }
                if (user == null)
                {
                    {
                        ModelState.AddModelError(string.Empty, "Username or Password is wrong");
                        return View(instructor);
                    }
                }

                // Par défaut, rediriger vers la page d'accueil :
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            Session.Remove("ID");
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}