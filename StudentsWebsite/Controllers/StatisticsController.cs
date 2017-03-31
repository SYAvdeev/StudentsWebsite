using Microsoft.AspNet.Identity.Owin;
using StudentsWebsite.Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentsWebsite.Models;

namespace StudentsWebsite.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllStudents()
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            List<Professor> professors = db.Professors.ToList();
            int numberOfStudents = db.Students.Count();

            List<Professor> professorsToView = new List<Professor>();

            foreach(var p in professors)
            {
                if(p.Enrollments.Count == numberOfStudents)
                {
                    professorsToView.Add(p);
                }
            }

            return View(professorsToView);
        }

        public ActionResult HighestGrade()
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            List<Enrollment> enrollments = db.Enrollments.ToList();
            double allAverage = enrollments.Average(e => e.Grade ?? 0);

            List<Student> students = db.Students.ToList();
            List<Student> studentsToView = new List<Student>();

            foreach(var s in students)
            {
                if (s.AverageGrade >= allAverage)
                    studentsToView.Add(s);
            }

            return View(studentsToView);
        }

        public ActionResult LessStudents()
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            List<Professor> professors = db.Professors.ToList();

            List<int> numbersOfStudents = new List<int>();
            foreach(var p in professors)
            {
                numbersOfStudents.Add(p.Enrollments.Count);
            }

            int minimalNumber = numbersOfStudents.Min();

            List<Professor> professorsToView = new List<Professor>();
            foreach (var p in professors)
            {
                if(p.Enrollments.Count == minimalNumber)
                {
                    professorsToView.Add(p);
                }
            }

            return View(professorsToView);
        }
    }
}