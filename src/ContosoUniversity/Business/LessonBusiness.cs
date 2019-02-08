using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContosoUniversity.Business
{
    public class LessonBusiness
    {
        //Déclaration du context
        private SchoolContext db;
        //Constructeur
        public LessonBusiness()
        {
            //Initialisation du context
            db = new SchoolContext();
        }

        //Methode renvoyant la liste des courses classé par ordre alphabetique
        public List<Course> CourseList()
        {
            return db.Courses.OrderBy(c => c.Title).ToList();
        }
        //Methode permettant de selectionner les lessons corespondant au cours selectionné
        public IQueryable<CourseDate> ListeFiltreLesson(int? selectedCourse, int courseId)
        {
            return db.CourseDates
                .Where(c => !selectedCourse.HasValue || c.CourseID == courseId)
                .OrderBy(c => c.CourseDateID);
        }
        //Methode permettant d'ajouter une lesson
        public void AddLesson(CourseDate lesson)
        {
            db.CourseDates.Add(lesson);
            db.SaveChanges();
        }
        //Methode de permettant de rechercher le lesson corespondant à l'id
        public CourseDate FindLesson (int? lessonId)
        {
            return db.CourseDates.Find(lessonId);
        }
        //Méthode permettant de supprimer une lesson de la base de données
        public void RemoveLesson(CourseDate lesson)
        {
            db.CourseDates.Remove(lesson);
            db.SaveChanges();
        }
        //Methode permettant de récupérer la date du premier cours s'il existe, sinon retour default DateTime
        public DateTime FirstCourseDate (int courseId)
        {
            try
            {
                CourseDate course = db.CourseDates.FirstOrDefault(c => c.CourseID == courseId);
                return course.FirstCourse;
            }
            catch (Exception)
            {
                return default(DateTime);
            }
        }
        //Methode permettant la sauvegarde
        public void SaveDataBase()
        {
            db.SaveChanges();
        }
    }
}