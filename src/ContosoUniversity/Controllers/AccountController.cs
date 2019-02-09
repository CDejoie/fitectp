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

        // Liste comportant les 2 types de Person
        readonly List<string> typePerson = new List<string>()
            {
               "Student",
               "Instructor"
            };

        //GET Register
        public ActionResult Register()
        {
            //Envoie des types de person à la vue
            ViewBag.typePerson = new SelectList(typePerson);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string ChoixPerson, string LastName, string FirstMidName, string Email, string UserName, string Password, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                //On vérifie que le username existe, s'il existe on renvoie une erreur
                if (accountBusiness.UserNameExist(UserName))
                {
                    ModelState.AddModelError("User Name", "User Name already exists.");
                    ViewBag.MessageDoublon = "The login " + UserName + " already exists. Try again";
                }
                else
                {   
                    //Sinon, si c'est un étudiant, on crée un étudiant
                    if (ChoixPerson == "Student")
                    {
                        accountBusiness.StudentRegistration(LastName, FirstMidName, Email, UserName, Password, ConfirmPassword);
                        ModelState.Clear();
                        ViewBag.Message =" " + FirstMidName + " " + LastName + " successfully registred.";
                    }
                    //Sinon, on crée un instructeur
                    else
                    {
                        accountBusiness.InstructorRegistration(LastName, FirstMidName, Email, UserName, Password, ConfirmPassword);
                        ModelState.Clear();
                        ViewBag.Message = " " +FirstMidName + " " + LastName + " successfully registred.";
                    }
                }

            }
            //En cas de message d'erreur on retour toujours la liste de type de person
            ViewBag.typePerson = new SelectList(typePerson);

            return View();
        }
      
        //GET Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Student student, Instructor instructor)
        {
            //Si la person logger a un EnrollmentDate (= c'est un étudiant)
            if (student.EnrollmentDate != default(DateTime))
            {
                //On tente de le logger
                Person user = accountBusiness.PeopleLogin(student.UserName, student.Password);

                //Si le userName correspond au password
                if (user != null)
                {
                    //On met dans une varibale de Session l'ID et le username
                    Session["ID"] = user.ID.ToString();
                    Session["UserName"] = user.UserName.ToString();

                    return RedirectToAction("Index", "Home");
                }
                //Sinon on renvoie un message d'erreur et en model l'étudiant qu'on a tenté d'enregistrer
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
            //Sinon (= si instructor)
            else
            {
                //On tente de le logger
                Person user = accountBusiness.PeopleLogin(instructor.UserName, instructor.Password);

                //Si le userName correspond au password
                if (user != null)
                {
                    //On met dans une varibale de Session l'ID et le username
                    Session["ID"] = user.ID.ToString();
                    Session["UserName"] = user.UserName.ToString();

                    return RedirectToAction("Index", "Home");
                }
                //Sinon on renvoie un message d'erreur et en model l'instructor qu'on a tenté d'enregistrer
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

        //Action permettant de se delogger
        public ActionResult LogOut()
        {
            //On supprime les variables de session
            Session.Remove("ID");
            Session.Remove("UserName");
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}