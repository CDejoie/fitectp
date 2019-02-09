using System;
using System.Data.Entity;
using System.Web.Mvc;
using ContosoUniversity.Business;
using ContosoUniversity.Controllers;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContosoUniversityTest
{
    [TestClass]
    public class AccountTests
    {
        //Déclaration de la classe business à tester
        private AccountBusiness businessToTest;
        //Déclaration de la classe controler à tester
        private AccountController controllerToTest;

        //Student pouvant être utilisé pour les tests
        public readonly Student studentTest = new Student()
        {
            ID = 1,
            LastName = "Dudouyt",
            FirstMidName = "Clement",
            Email = "c.dudouyt@gmail.com",
            UserName = "CDudouyt",
            Password = "123456",
            ConfirmPassword = "123456",
            EnrollmentDate = DateTime.Now
        };

        //Student pouvant être utilisé pour les tests
        public readonly Student studentTest2 = new Student()
        {
            ID = 1,
            LastName = "Dudouyt",
            FirstMidName = "Clement",
            Email = "c.dudouyt@gmail.com",
            UserName = "CDudouyt",
            Password = "123456",
            ConfirmPassword = "123456",
        };

        //Instructor pouvant être utilisé pour les tests
        public readonly Instructor instructorTest = new Instructor()
        {
            ID = 1,
            LastName = "Dejoie",
            FirstMidName = "Clement",
            Email = "c.dejoie@gmail.com",
            UserName = "CDejoie",
            Password = "123456",
            ConfirmPassword = "123456",
            HireDate = DateTime.Now
        };

        //Classe de test qui se lancera A CHAQUE DEBUT DE NOUVELLE METHODE DE TEST
        [TestInitialize]
        public void Init_AvantChaqueTest()
        {
            //Initialisation de la classe business
            businessToTest = new AccountBusiness();
            //Initialisation de la classe controller
            controllerToTest = new AccountController();

            //Supression (si besoin), création et initialisation de la base de donnée via le context "SchoolContext"
            IDatabaseInitializer<SchoolContext> init = new DropCreateDatabaseAlways<SchoolContext>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new SchoolContext());
        }

        //Classe de test qui se lancera A CHAQUE FIN METHODE DE TEST
        [TestCleanup]
        public void ApresChaqueTest()
        {
            businessToTest.Dispose();
        }

        //Test permettant de verifier que la methode StudentRegistration renvoie bien un student en verifiant via la methode UserNameExist
        [TestMethod]
        public void StudentRegistration_AvecUnNouvelEtudiant_LeUsernameABienEteCree()
        {
            businessToTest.StudentRegistration("Durand", "Paul", "paul.durand@gmail.com", "PDurand", "azerty", "azerty");

            Assert.IsTrue(businessToTest.UserNameExist("PDurand"));
        }

        //Test permettant de verifier que la methode InstructorRegistration renvoie bien un student en verifiant via la methode UserNameExist
        [TestMethod]
        public void InstructorRegistration_AvecUnNouveauProfesseur_LeUsernameABienEteCree()
        {
            businessToTest.StudentRegistration("Dupont", "Juliette", "juliette.dupont@gmail.com", "JDupont", "123456", "123456");

            Assert.IsTrue(businessToTest.UserNameExist("JDupont"));
        }

        //Test de la methode register en verifiant via la methode PeopleLogin
        [TestMethod]
        public void PeopleLogin_AvecBonMDPEtUsername_RetourneUnePersonne()
        {
            controllerToTest.Register("Student", "Dudouyt", "Clement", "c.dudouyt@gmail.com", "CDudouyt", "123456", "123456");
            Person personRegister = businessToTest.PeopleLogin("CDudouyt", "123456");

            Assert.AreEqual("CDudouyt", personRegister.UserName);
            Assert.AreEqual("123456", personRegister.Password);
            Assert.AreEqual("c.dudouyt@gmail.com", personRegister.Email);
        }

        //Test de la methode register en verifiant via la methode PeopleLogin (avec une erreur de Login)
        [TestMethod]
        public void PeopleLogin_AvecMauvaisMDPEtUnBonUsername_RetourneNull()
        {
            controllerToTest.Register("Student", "Dudouyt", "Clement", "c.dudouyt@gmail.com", "CDudouyt", "123456", "123456");
            Person personRegister = businessToTest.PeopleLogin("CDudouyt", "1234567");

            Assert.IsNull(personRegister);
        }

        //Test de la methode register en verifiant via la methode PeopleLogin (avec une erreur de Login)
        [TestMethod]
        public void PeopleLogin_AvecBonMDPEtUnMauvaisUsername_RetourneNull()
        {
            controllerToTest.Register("Student", "Dudouyt", "Clement", "c.dudouyt@gmail.com", "CDudouyt", "123456", "123456");
            Person personRegister = businessToTest.PeopleLogin("CDudouyt44", "123456");

            Assert.IsNull(personRegister);
        }


        //Test me senblant sans intérêt : on test que l'action du controller retourne une vue vide

        //[TestMethod]
        //public void AccountController_LoginGet_RenvoiVueRegister()
        //{
        //    ViewResult resultat = (ViewResult)controllerToTest.Login();

        //    Assert.AreEqual("", resultat.ViewName);
        //}
        //[TestMethod]
        //public void AccountController_RegisterGet_RenvoiVueRegister()
        //{
        //    ViewResult resultat = (ViewResult)controllerToTest.Register();

        //    Assert.AreEqual("", resultat.ViewName);
        //}

        //Test de l'action register avec un etudiant qui s'enregistre bien
        [TestMethod]
        public void AccountController_RegisterPostStudent_RenvoiViewBag()
        {
            ViewResult resultat = (ViewResult)controllerToTest.Register("Student", "Dudouyt", "Clement", "c.dudouyt@gmail.com", "CDudouyt", "123456", "123456");

            Assert.AreEqual(" Clement Dudouyt successfully registred.", resultat.ViewBag.Message);
        }

        //Test de l'action register avec un etudiant qui ne s'enregistre pas
        [TestMethod]
        public void AccountController_RegisterPostStudentAlreadyExist_RenvoiViewBag()
        {
            controllerToTest.Register("Student", "Dudouyt", "Clement", "c.dudouyt@gmail.com", "CDudouyt", "123456", "123456");
            ViewResult resultat = (ViewResult)controllerToTest.Register("Student", "Dudouyt", "Clement", "c.dudouyt@gmail.com", "CDudouyt", "123456", "123456");

            Assert.AreEqual("The login CDudouyt already exists. Try again", resultat.ViewBag.MessageDoublon);
        }

        //Test dont le code est bon, mais pour lequel on a un probleme lié aux variables de session
        //[TestMethod]
        //public void AccountController_LoginPostStudentAlreadyExist_RedirectToIndexHome()
        //{
        //    controllerToTest.Register("Student", "Dudouyt", "Clement", "c.dudouyt@gmail.com", "CDudouyt", "123456", "123456");
        //    RedirectToRouteResult resultat = (RedirectToRouteResult)controllerToTest.Login(studentTest, null);

        //    Assert.AreEqual("Index", resultat.RouteValues["action"]);
        //    Assert.AreEqual("Home", resultat.RouteValues["controller"]);
        //}

        //Test me senblant sans intérêt : on test que l'action du controller retourne une vue vide
        //[TestMethod]
        //public void AccountController_RegisterPostStudentWrongUserName_RenvoieVueRegister()
        //{
        //    controllerToTest.Register("Student", "Dudouyt", "Clement", "c.dudouyt@gmail.com", "CDudouyt", "1234567", "1234567");
        //    ViewResult resultat = (ViewResult)controllerToTest.Login(studentTest, null);

        //    Assert.AreEqual("", resultat.ViewName);
        //}

        //Test de l'action register avec un instructor qui s'enregistre bien
        [TestMethod]
        public void AccountController_RegisterPostInstructor_RenvoiViewBag()
        {
            ViewResult resultat = (ViewResult)controllerToTest.Register("Instructor", "Dejoie", "Clement", "c.dejoie@gmail.com", "CDejoie", "123456", "123456");

            Assert.AreEqual(" Clement Dejoie successfully registred.", resultat.ViewBag.Message);
        }

        //Test de l'action register avec un instructor qui ne s'enregistre pas
        [TestMethod]
        public void AccountController_RegisterPostInstructorAlreadyExist_RenvoiViewBag()
        {
            controllerToTest.Register("Instructor", "Dejoie", "Clement", "c.dejoie@gmail.com", "CDejoie", "123456", "123456");
            ViewResult resultat = (ViewResult)controllerToTest.Register("Instructor", "Dejoie", "Clement", "c.dejoie@gmail.com", "CDejoie", "123456", "123456");

            Assert.AreEqual("The login CDejoie already exists. Try again", resultat.ViewBag.MessageDoublon);
        }

        //Test dont le code est bon, mais pour lequel on a un probleme lié aux variables de session
        //[TestMethod]
        //public void AccountController_LoginPostInstructorAlreadyExist_RedirectToIndexHome()
        //{
        //    controllerToTest.Register("Instructor", "Dejoie", "Clement", "c.dejoie@gmail.com", "CDejoie", "123456", "123456");
        //    RedirectToRouteResult resultat = (RedirectToRouteResult)controllerToTest.Login(studentTest2, instructorTest);

        //    Assert.AreEqual("Index", resultat.RouteValues["action"]);
        //    Assert.AreEqual("Home", resultat.RouteValues["controller"]);
        //}
    }
}

