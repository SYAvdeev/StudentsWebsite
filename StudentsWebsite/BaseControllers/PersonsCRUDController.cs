using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsWebsite.BaseControllers
{
    public abstract class PersonsCRUDController : Controller
    {
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
            foreach (var s in stList)
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
    }
}