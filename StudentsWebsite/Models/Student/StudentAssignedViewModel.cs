using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsWebsite.Models
{
    public class StudentAssignedViewModel
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
        public int? Grade { get; set; }
    }
}