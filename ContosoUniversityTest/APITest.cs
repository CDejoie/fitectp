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
        private AccountBusiness accountBusiness;
        private StudentsController controllerToTest;

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

        [TestInitialize]
        public void Init_AvantChaqueTest()
        {
            accountBusiness = new AccountBusiness();
            controllerToTest = new StudentsController();

            IDatabaseInitializer<SchoolContext> init = new DropCreateDatabaseAlways<SchoolContext>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new SchoolContext());
        }

        [TestCleanup]
        public void ApresChaqueTest()
        {
            accountBusiness.Dispose();
        }

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
