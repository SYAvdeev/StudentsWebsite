using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentsWebsite.Models;

namespace StudentsWebsite.Data_Access_Layer
{
    public class UniversityInitializer : System.Data.Entity.DropCreateDatabaseAlways<UniversityContext>
    {
        protected override void Seed(UniversityContext context)
        {
            var students = new List<Student>
            {
                new Student { FirstName = "Сергей", LastName = "Авдеев" },
                new Student { FirstName = "Николай", LastName = "Нагирный" },
                new Student { FirstName = "Базар", LastName = "Базаров" },
                new Student { FirstName = "Нина", LastName = "Селезнёва" }
            };

            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();

            var professors = new List<Professor>
            {
                new Professor { FirstName = "Константин", LastName = "Лысаков"},
                new Professor { FirstName = "Александр", LastName = "Золкин" },
                new Professor { FirstName = "Ирина", LastName = "Долгунцева" }
            };

            professors.ForEach(p => context.Professors.Add(p));
            context.SaveChanges();

            var courses = new List<Course>
            {
                new Course { Name = "Введение в АФТИ", ProfessorID = 1 },
                new Course { Name = "Измерительный практикум", ProfessorID = 2 },
                new Course { Name = "Аналитическая геометрия", ProfessorID = 3 }
            };

            courses.ForEach(c => context.Courses.Add(c));
            context.SaveChanges();
            
            var enrollments = new List<Enrollment>
            {
                new Enrollment { StudentID = 1, CourseID = 1, Mark = 5 },
                new Enrollment { StudentID = 1, CourseID = 2, Mark = 5 },
                new Enrollment { StudentID = 1, CourseID = 3, Mark = 5 },
                new Enrollment { StudentID = 2, CourseID = 1, Mark = 2 },
                new Enrollment { StudentID = 2, CourseID = 2, Mark = 3 },
                new Enrollment { StudentID = 2, CourseID = 3, Mark = 4 },
                new Enrollment { StudentID = 3, CourseID = 1, Mark = 3 },
                new Enrollment { StudentID = 3, CourseID = 2, Mark = 2 },
                new Enrollment { StudentID = 3, CourseID = 3, Mark = 5 },
                new Enrollment { StudentID = 4, CourseID = 1, Mark = 3 },
                new Enrollment { StudentID = 4, CourseID = 2, Mark = 2 },
                new Enrollment { StudentID = 4, CourseID = 3, Mark = 5 },
            };

            enrollments.ForEach(e => context.Enrollments.Add(e));
            context.SaveChanges();
        }
    }
}