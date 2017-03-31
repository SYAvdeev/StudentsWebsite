using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsWebsite.Models
{
    public class ProfessorRegisterViewModel : RegisterViewModel
    {
        [Required]
        [Display(Name = "Course")]
        public string Course { get; set; }
    }
}