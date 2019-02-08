using ContosoUniversity.DAL;
using ContosoUniversity.DTO;
using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace ContosoUniversity.AuthData
{
    public class AuthenticationFilter : ActionFilterAttribute, IActionFilter

    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Student student = new Student();
            SchoolContext db = new SchoolContext();
            if (student.EnrollmentDate == default(DateTime))
            {
                if (HttpContext.Current.Session["ID"] == null)
                {

                    {
                        filterContext.Result = new HttpUnauthorizedResult();
                    }
                    base.OnActionExecuting(filterContext);
                }
            }

        }


    }
    }

