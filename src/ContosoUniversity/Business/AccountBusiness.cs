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
    public static class AccountBusiness
    {
        public static void RegistredConfirmationStudent( string ChoixPerson, string LastName, string FirstMidName, string Email, string UserName, string Password, string ConfirmPassword)
        {
            SchoolContext db = new SchoolContext();
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
    

    public static void RegistredConfirmationInstructor(string ChoixPerson, string LastName, string FirstMidName, string Email, string UserName, string Password, string ConfirmPassword)
    {
        SchoolContext db = new SchoolContext();
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
    }
 
}


    