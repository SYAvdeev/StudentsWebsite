using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsWebsite.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentID { get; set; }
        [Range(2, 5)]
        [DisplayFormat(NullDisplayText = "None")]
        public int? Grade { get; set; }
        public int ProfessorID { get; set; }
        public int StudentID { get; set; }

        public virtual Professor Professor { get; set; }
        public virtual Student Student { get; set; }
    }
}