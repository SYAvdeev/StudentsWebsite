using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsWebsite.Models
{
    public class StudentsIndexViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Name")]
        public string FullName { get; set; }
        [Display(Name = "Average Grade")]
        public float AverageGrade { get; set; }
    }
}