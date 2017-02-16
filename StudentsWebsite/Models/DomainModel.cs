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
        public string Name { get; set; }
    }

    public class CourseWithMark
    {
        public Course Course { get; }
        public int Mark { get; }

        public CourseWithMark(Course course, int mark)
        {
            Course = course;
            Mark = mark;
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Student : Person
    {
        public virtual ICollection<CourseWithMark> CoursesWithMarks { get; set; }
    }

    public class Professor : Person
    {
        public Course Course { get; set; }
    }

    public class UniversityDB : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Professor> Professors { get; set; }
    }
}