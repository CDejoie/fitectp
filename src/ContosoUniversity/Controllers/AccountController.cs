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
        AccountBusiness accountBusiness = new AccountBusiness();

        // Envoie d'une liste comportant les 2 types de Person à la vue "Register"
        readonly List<string> typePerson = new List<string>()
            {
               "Student",
               "Instructor"
            };

        public ActionResult Register()
        {
            ViewBag.typePerson = new SelectList(typePerson);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string ChoixPerson, string LastName, string FirstMidName, string Email, string UserName, string Password, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (accountBusiness.UserNameExist(UserName))
                {
                    ModelState.AddModelError("User Name", "User Name already exists.");
                    ViewBag.MessageDoublon = "The login " + UserName + " already exists. Try again";
                }
                else
                {
                    if (ChoixPerson == "Student")
                    {
                        accountBusiness.StudentRegistration(LastName, FirstMidName, Email, UserName, Password, ConfirmPassword);
                        ModelState.Clear();
                        ViewBag.Message =" " + FirstMidName + " " + LastName + " successfully registred.";
                    }
                    else
                    {
                        accountBusiness.InstructorRegistration(LastName, FirstMidName, Email, UserName, Password, ConfirmPassword);
                        ModelState.Clear();
                        ViewBag.Message = " " +FirstMidName + " " + LastName + " successfully registred.";
                    }
                }

            }
            ViewBag.typePerson = new SelectList(typePerson);

            return View();
        }
      
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Student student, Instructor instructor)
        {
            if (student.EnrollmentDate != default(DateTime))
            {
                Person user = accountBusiness.PeopleLogin(student.UserName, student.Password);
                if (user != null)
                {
                    //Session["ID"] = user.ID.ToString();
                    //Session["UserName"] = user.UserName.ToString();
                    return RedirectToAction("Index", "Home");
                }
                if (user == null)
                {
                    {
                        ModelState.AddModelError("", "Username or Password is wrong");
                        return View(student);
                    }
                }

                // Par défaut, rediriger vers la page d'accueil :
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Person user = accountBusiness.PeopleLogin(instructor.UserName, instructor.Password); ;
                if (user != null)
                {
                    //Session["ID"] = user.ID.ToString();
                    //Session["UserName"] = user.UserName.ToString();
                    return RedirectToAction("Index", "Home");
                }
                if (user == null)
                {
                    {
                        ModelState.AddModelError("", "Username or Password is wrong");
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
            Session.Remove("UserName");
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}