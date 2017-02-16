using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentsWebsite.Models;

namespace StudentsWebsite.Controllers
{
    public class HomeController : Controller
    {
        UniversityDB univDB = new UniversityDB();

        public ActionResult Index()
        {
            return View(univDB.Students);
        }

        protected override void Dispose(bool disposing)
        {
            univDB.Dispose();
            base.Dispose(disposing);
        }
    }
}