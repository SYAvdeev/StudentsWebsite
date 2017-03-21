using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using StudentsWebsite.Data_Access_Layer;
using StudentsWebsite.Models;
using PagedList;
using Microsoft.AspNet.Identity.Owin;
using StudentsWebsite.Data_Access_Layer;

namespace StudentsWebsite.Controllers
{
    public class StudentsController : Controller
    {
        // GET: Students
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.GradeSortParam = sortOrder == "grade" ? "grade_desc" : "grade";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            List<Student> stList = db.Students.ToList();
            
            List<StudentsIndexViewModel> students = new List<StudentsIndexViewModel>();
            foreach(var s in stList)
            {
                students.Add(new StudentsIndexViewModel() { ID = s.ID, FullName = s.FullName, AverageGrade = s.AverageGrade });
            }
            
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.FullName.Contains(searchString)).ToList();
            }

            IOrderedEnumerable<StudentsIndexViewModel> orderedStudents;

            switch (sortOrder)
            {
                case "name_desc":
                    orderedStudents = students.OrderByDescending(s => s.FullName);
                    break;
                case "grade_desc":
                    orderedStudents = students.OrderByDescending(s => s.AverageGrade);
                    break;
                case "grade":
                    orderedStudents = students.OrderBy(s => s.AverageGrade);
                    break;
                default:
                    orderedStudents = students.OrderBy(s => s.FullName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(orderedStudents.ToPagedList(pageNumber, pageSize));
        }



        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Enrollments")] Student student)
        {
            if (ModelState.IsValid)
            {
                var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,FirstName,LastName")] Student student)
        {
            if (ModelState.IsValid)
            {
                var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
