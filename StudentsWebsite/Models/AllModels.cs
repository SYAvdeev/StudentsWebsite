using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace StudentsWebsite.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }
        public string Name { get; set; }
        public int ProfessorID { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }

    public class Enrollment
    {
        [Key]
        public int EnrollmentID { get; set; }
        public int Mark { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Student : Person
    {
        [Key]
        public int StudentID { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }

    public class Professor : Person
    {
        [Key]
        public int ProfessorID { get; set; }
        public int CourseID { get; set; }
    }
}