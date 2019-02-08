using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.DTO;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class StudentsController : ApiController
    {
        private SchoolContext db = new SchoolContext();

        // GET: api/Students/5
        //[ResponseType(typeof(Student))]
     
        public IHttpActionResult GetStudent(int id)
        {
            try
            {
                //Création d'un étudiant et de la mise en relation avec l'ID
                Student student = db.Students.FirstOrDefault(x => x.ID == id);
                // Création d'un student DTO ou semble il un view model d'après mes documentations
                StudentsDTO studentDTO = new StudentsDTO();
                List<Dictionary<string, string>> courseList = new List<Dictionary<string, string>>();

                // parcours Enrollment
                foreach (Enrollment enrollment in student.Enrollments)
                {
                    // Création du dictionnaire course
                    Dictionary<string, string> course = new Dictionary<string, string>();
                    // Parcours l'enrollment par le course ID
                    course["CourseID"] = enrollment.CourseID.ToString();
                    // ajoute une un cours  au dictionnaire lorsqur CoursID Correspond
                    courseList.Add(course);
                }
                //Affichage des données
                studentDTO.id = student.ID;
                studentDTO.firstMidName = student.FirstMidName;
                studentDTO.lastName = student.LastName;
                studentDTO.enrollmentDate = student.EnrollmentDate;
                studentDTO.enrollments = courseList;

                return Ok(studentDTO);
            }
            catch
            {
                return NotFound();

            }
        }
    }
}