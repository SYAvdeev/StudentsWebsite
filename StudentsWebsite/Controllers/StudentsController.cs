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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StudentsWebsite.Data_Access_Layer;
using System.Threading.Tasks;

namespace StudentsWebsite.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private ApplicationUserManager _userManager;

        public StudentsController() { }

        public StudentsController(ApplicationUserManager userManager)
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

        // GET: Students
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [Authorize(Roles ="Admin,Professor")]
        public ActionResult Create()
        {
            var student = new Student();
            student.Enrollments = new List<Enrollment>();
            PopulateAssignedCourses(student);
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Professor")]
        public async Task<ActionResult> Create( RegisterViewModel registerModel, string[] selectedProfessors, string[] grades)
        {
            var student = new Student() { FirstName = registerModel.FirstName, LastName = registerModel.LastName };
            student.Enrollments = new List<Enrollment>();

            if (ModelState.IsValid)
            {
                if (selectedProfessors != null)
                {
                    foreach (var professor in selectedProfessors)
                    {
                        Enrollment newEnrollment = new Enrollment();
                        newEnrollment.Student = student;
                        int i = int.Parse(professor);
                        newEnrollment.ProfessorID = i;
                        if (!string.IsNullOrEmpty(grades[i - 1]))
                            newEnrollment.Grade = int.Parse(grades[i - 1]);

                        student.Enrollments.Add(newEnrollment);
                    }
                }

                var user = new ApplicationUser { UserName = registerModel.Login };
                await UserManager.CreateAsync(user, registerModel.Password);
                student.User = UserManager.FindByName(registerModel.Login);
                var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                db.Students.Add(student);
                db.SaveChanges();
            }

            PopulateAssignedCourses(student);
            return RedirectToAction("Index");
        }

        private void PopulateAssignedCourses(Student student)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var allProfessors = db.Professors.ToList();
            var studentsProfessors = new HashSet<int>(student.Enrollments.Select(c => c.ProfessorID));
            var viewModel = new List<ProfessorAssignedViewModel>();
            foreach (var professor in allProfessors)
            {
                bool contains = studentsProfessors.Contains(professor.ID);
                viewModel.Add(new ProfessorAssignedViewModel
                {
                    ProfessorID = professor.ID,
                    Course = professor.Course,
                    Assigned = contains,
                    Grade = contains ? professor.Enrollments.Where(e => e.StudentID == student.ID).Select(e => e.Grade).FirstOrDefault() : null
                });
            }
            ViewBag.Professors = viewModel;
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin,Professor")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            Student student = db.Students.Find(id);
            ApplicationUser user = student.User;
            StudentEditViewModel editModel = new StudentEditViewModel()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Login = student.User.UserName,
                Password = "",
                ConfirmPassword = "",
                StudentID = student.ID
            };

            if (student == null)
            {
                return HttpNotFound();
            }
            PopulateAssignedCourses(student);
            return View(editModel);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Professor")]
        public async Task<ActionResult> Edit(RegisterViewModel registerModel, string[] selectedProfessors, string[] grades, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var studentToUpdate = db.Students
               .Include(i => i.Enrollments)
               .Where(i => i.ID == id)
               .Single();

            if (TryUpdateModel(studentToUpdate, "", new string[] { "LastName", "FirstName" }))
            {
                try
                {
                    UpdateStudentEnrollments(selectedProfessors, studentToUpdate);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }



                var student = new Student() { FirstName = registerModel.FirstName, LastName = registerModel.LastName };
            student.Enrollments = new List<Enrollment>();

            


            if (ModelState.IsValid)
            {
                if (selectedProfessors != null)
                {
                    foreach (var professor in selectedProfessors)
                    {
                        Enrollment newEnrollment = new Enrollment();
                        newEnrollment.Student = student;
                        int i = int.Parse(professor);
                        newEnrollment.ProfessorID = i;
                        if (!string.IsNullOrEmpty(grades[i - 1]))
                            newEnrollment.Grade = int.Parse(grades[i - 1]);

                        student.Enrollments.Add(newEnrollment);
                    }
                }

                var user = new ApplicationUser { UserName = registerModel.Login };
                await UserManager.CreateAsync(user, registerModel.Password);
                student.User = UserManager.FindByName(registerModel.Login);
                

                var studentToUpdate = db.Students.Find(id);
                TryUpdateModel(studentToUpdate, )

                db.Students.Add(student);
                db.SaveChanges();
            }

            PopulateAssignedCourses(student);
            return RedirectToAction("Index");
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin,Professor")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            Student student = db.Students.Find(id);
            //UserManager.
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Professor")]
        public ActionResult DeleteConfirmed(int id)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            Student student = db.Students.Find(id);
            var user = UserManager.FindById(student.UserID);
            if(user != null)
                UserManager.Delete(user);

            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
