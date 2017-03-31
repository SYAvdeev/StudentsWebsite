using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsWebsite.Models
{
    public class Professor : Person
    {
        public string Course { get; set; }
        public int NumOfStudents { get { return Enrollments.Count; } }
    }
}