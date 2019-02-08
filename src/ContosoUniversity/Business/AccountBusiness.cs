using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace ContosoUniversity.Business
{
    public class AccountBusiness
    {
        private SchoolContext db;
        public AccountBusiness()
        {
            db = new SchoolContext();
        }

        //Methode permettant de vérifier que le userName existe
        public bool UserNameExist(string username)
        {
            if (db.People.Any(x => x.UserName == username)) return true;
            else return false;
        }

        //Methode permettant d'enregistrer un étudiant
        public void StudentRegistration(string LastName, string FirstMidName, string Email, string UserName, string Password, string ConfirmPassword)
        {
            db.Students.Add(new Student
            {
                LastName = LastName,
                FirstMidName = FirstMidName,
                Email = Email,
                UserName = UserName,
                Password = Password,
                ConfirmPassword = ConfirmPassword,
                EnrollmentDate = DateTime.Now
            });
            db.SaveChanges();
        }

        //Methode permettant d'enregistrer un instructeur
        public void InstructorRegistration(string LastName, string FirstMidName, string Email, string UserName, string Password, string ConfirmPassword)
        {
            db.Instructors.Add(new Instructor
            {
                LastName = LastName,
                FirstMidName = FirstMidName,
                Email = Email,
                UserName = UserName,
                Password = Password,
                ConfirmPassword = ConfirmPassword,
                HireDate = DateTime.Now
            });
            db.SaveChanges();
        }

        //Methode permettant de verifier userName et password
        public Person PeopleLogin(string userName, string password)
        {
            return db.People.SingleOrDefault(x => x.UserName == userName && x.Password == password);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}


    