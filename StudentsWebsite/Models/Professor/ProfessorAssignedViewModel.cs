using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsWebsite.Models
{
    public class ProfessorAssignedViewModel
    {
        public int ProfessorID { get; set; }
        public string Course { get; set; }
        public bool Assigned { get; set; }
        public int? Grade { get; set; }
    }
}