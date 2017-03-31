using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentsWebsite.Models
{
    public abstract class Person
    {
        public int ID { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "Name")]
        public string FullName { get { return FirstName + " " + LastName; } }

        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; } 

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}