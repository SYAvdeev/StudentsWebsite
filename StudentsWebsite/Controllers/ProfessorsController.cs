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
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace StudentsWebsite.Controllers
{
    [Authorize]
    public class ProfessorsController : Controller
    {
        private ApplicationUserManager _userManager;

        public ProfessorsController() { }

        public ProfessorsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [AllowAnonymous]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.StudentsNumParam = sortOrder == "grade" ? "grade_desc" : "grade";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            List<Professor> prList = db.Professors.ToList();

            List<ProfessorIndexViewModel> professors = new List<ProfessorIndexViewModel>();
            foreach (var p in prList)
            {
                professors.Add(new ProfessorIndexViewModel() { ID = p.ID, FullName = p.FullName, NumOfStudents = p.NumOfStudents });
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                professors = professors.Where(p => p.FullName.Contains(searchString)).ToList();
            }

            IOrderedEnumerable<ProfessorIndexViewModel> orderedProfessors;

            switch (sortOrder)
            {
                case "name_desc":
                    orderedProfessors = professors.OrderByDescending(p => p.FullName);
                    break;
                case "grade_desc":
                    orderedProfessors = professors.OrderByDescending(p => p.NumOfStudents);
                    break;
                case "grade":
                    orderedProfessors = professors.OrderBy(p => p.NumOfStudents);
                    break;
                default:
                    orderedProfessors = professors.OrderBy(p => p.FullName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(orderedProfessors.ToPagedList(pageNumber, pageSize));
        }



        // GET: Students/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = db.Professors.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        // GET: Professors/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            var professor = new Professor();
            professor.Enrollments = new List<Enrollment>();
            PopulateAssignedStudents(professor);
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(ProfessorRegisterViewModel registerModel, string[] selectedStudents, string[] grades)
        {
            var professor = new Professor() { FirstName = registerModel.FirstName, LastName = registerModel.LastName, Course = registerModel.Course };
            professor.Enrollments = new List<Enrollment>();

            if (ModelState.IsValid)
            {
                if (selectedStudents != null)
                {
                    foreach (var student in selectedStudents)
                    {
                        Enrollment newEnrollment = new Enrollment();
                        newEnrollment.Professor = professor;
                        int i = int.Parse(student);
                        newEnrollment.StudentID = i;
                        if (!string.IsNullOrEmpty(grades[i - 1]))
                            newEnrollment.Grade = int.Parse(grades[i - 1]);

                        professor.Enrollments.Add(newEnrollment);
                    }
                }

                var user = new ApplicationUser { UserName = registerModel.Login };
                await UserManager.CreateAsync(user, registerModel.Password);
                professor.User = UserManager.FindByName(registerModel.Login);
                var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                db.Professors.Add(professor);
                db.SaveChanges();
            }

            PopulateAssignedStudents(professor);
            return RedirectToAction("Index");
        }

        private void PopulateAssignedStudents(Professor professor)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var allStudents = db.Students.ToList();
            var students = new HashSet<int>(professor.Enrollments.Select(c => c.StudentID));
            var viewModel = new List<StudentAssignedViewModel>();
            foreach (var student in allStudents)
            {
                viewModel.Add(new StudentAssignedViewModel
                {
                    StudentID = student.ID,
                    Name = student.FullName,
                    Assigned = students.Contains(student.ID)
                });
            }
            ViewBag.Students = viewModel;
        }

        // GET: Professors/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            Professor professor = db.Professors.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        // POST: Professors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "professorID,FirstName,LastName")] Professor professor)
        {
            if (ModelState.IsValid)
            {
                var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                db.Entry(professor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(professor);
        }

        // GET: Professors/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            Professor professor = db.Professors.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        // POST: Professors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            Professor professor = db.Professors.Find(id);
            db.Professors.Remove(professor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
