using System;
using System.Data.Entity;
using ContosoUniversity.Business;
using ContosoUniversity.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContosoUniversityTest
{
    [TestClass]
    public class AccountTests
    {
        private AccountBusiness aB;

        [TestInitialize]
        public void Init_AvantChaqueTest()
        {
            aB = new AccountBusiness();

            IDatabaseInitializer<SchoolContext> init = new DropCreateDatabaseAlways<SchoolContext>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new SchoolContext());
        }

        [TestCleanup]
        public void ApresChaqueTest()
        {
            aB.Dispose();
        }

        [TestMethod]
        public void StudentRegistration_AvecUnNouvelEtudiant_LeUsernameABienEteCree()
        {
            aB.StudentRegistration("Durand", "Paul", "paul.durand@gmail.com", "PDurand", "azerty", "azerty");

            Assert.IsTrue(aB.UserNameExist("PDurand"));
        }

        [TestMethod]
        public void InstructorRegistration_AvecUnNouveauProfesseur_LeUsernameABienEteCree()
        {
            aB.StudentRegistration("Dupont", "Juliette", "juliette.dupont@gmail.com", "JDupont", "123456", "123456");

            Assert.IsTrue(aB.UserNameExist("JDupont"));
        }
    }
}

