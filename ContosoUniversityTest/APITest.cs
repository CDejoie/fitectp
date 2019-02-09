using ContosoUniversity.Business;
using ContosoUniversity.Controllers;
using ContosoUniversity.DAL;
using ContosoUniversity.DTO;
using ContosoUniversity.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Http.Results;

namespace ContosoUniversityTest
{
    [TestClass]
    public class APITest
    {
        //Déclaration de la classe business utile pour le test
        private AccountBusiness accountBusiness;
        //Déclaration de la classe controler à tester
        private StudentsController controllerToTest;

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
            accountBusiness = new AccountBusiness();
            //Initialisation de la classe controller
            controllerToTest = new StudentsController();

            //Supression (si besoin), création et initialisation de la base de donnée via le context "SchoolContext"
            IDatabaseInitializer<SchoolContext> init = new DropCreateDatabaseAlways<SchoolContext>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new SchoolContext());
        }

        //Classe de test qui se lancera A CHAQUE FIN METHODE DE TEST
        [TestCleanup]
        public void ApresChaqueTest()
        {
            accountBusiness.Dispose();
        }

        //On verifie que l'API GetStudent retourne un OK()
        [TestMethod]
        public void GetStudent_SurStudentExistantSansCours_RetourneTypeOK()
        {
            // Arrange
            accountBusiness.StudentRegistration("Dudouyt", "Clement", "c.dudouyt@gmail.com", "CDudouyt", "123456", "123456");

            // Act
            var okResult = controllerToTest.GetStudent(1);

            // Assert
            Assert.IsInstanceOfType(okResult, typeof(OkNegotiatedContentResult<StudentsDTO>));
        }

        //On test que l'API retourne bien le bon contenu
        [TestMethod]
        public void GetStudent_SurStudentExistantSansCours_RetourneJSON()
        {
            // Arrange
            accountBusiness.StudentRegistration("Dudouyt", "Clement", "c.dudouyt@gmail.com", "CDudouyt", "123456", "123456");

            // Act
            IHttpActionResult okResult = controllerToTest.GetStudent(1);
            var contentResult = okResult as OkNegotiatedContentResult<StudentsDTO>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.id);
            Assert.AreEqual("Dudouyt", contentResult.Content.lastname);
            Assert.AreEqual("Clement", contentResult.Content.firstname);
        }
    }
}
