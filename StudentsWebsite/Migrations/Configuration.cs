namespace StudentsWebsite.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using StudentsWebsite.Data_Access_Layer;
    using Microsoft.Owin;
    using Owin;

    internal sealed class Configuration : DbMigrationsConfiguration<StudentsWebsite.Data_Access_Layer.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StudentsWebsite.Data_Access_Layer.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var roleStore = new RoleStore<ApplicationRole>(context);
            var roleManager = new RoleManager<ApplicationRole>(roleStore);
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);


            CreateRole("Admin", roleManager);
            CreateRole("Student", roleManager);
            CreateRole("Professor", roleManager);

            CreateUser("admin", "1111111", "Admin", userManager);

            CreateUser("s.avdeev", "2222222", "Student", userManager);
            CreateUser("n.nagirny", "3333333", "Student", userManager);
            CreateUser("b.bazarov", "4444444", "Student", userManager);
            CreateUser("n.selezneva", "5555555", "Student", userManager);

            CreateUser("k.lysakov", "6666666", "Professor", userManager);
            CreateUser("i.dolguntseva", "7777777", "Professor", userManager);
            CreateUser("a.zolkin", "8888888", "Professor", userManager);

            var students = new List<Student>()
            {
                new Student { FirstName = "Sergey", LastName = "Avdeev", UserID = userManager.FindByName("s.avdeev").Id },
                new Student { FirstName = "Nicolas", LastName = "Nagirny", UserID = userManager.FindByName("n.nagirny").Id },
                new Student { FirstName = "Bazar", LastName = "Bazarov", UserID = userManager.FindByName("b.bazarov").Id },
                new Student { FirstName = "Nina", LastName = "Selezneva", UserID = userManager.FindByName("n.selezneva").Id }
            };

            students.ForEach(s => context.Students.AddOrUpdate(s1 => s1.LastName, s));
            context.SaveChanges();

            var professors = new List<Professor>()
            {
                new Professor { FirstName = "Konstantin", LastName = "Lysakov", Course = "Introduction to APTR", UserID = userManager.FindByName("k.lysakov").Id },
                new Professor { FirstName = "Alexandr", LastName = "Zolkin", Course = "Matan", UserID = userManager.FindByName("a.zolkin").Id },
                new Professor { FirstName = "Irina", LastName = "Dolguntseva", Course = "Algebra", UserID = userManager.FindByName("i.dolguntseva").Id }
            };

            professors.ForEach(p => context.Professors.AddOrUpdate(p1 => p1.LastName, p));
            context.SaveChanges();


            var enrollments = new List<Enrollment>()
            {
                new Enrollment { StudentID = 1, ProfessorID = 1, Grade = 5 },
                new Enrollment { StudentID = 1, ProfessorID  = 2, Grade = 5 },
                new Enrollment { StudentID = 1, ProfessorID  = 3, Grade = 5 },
                new Enrollment { StudentID = 2, ProfessorID  = 1, Grade = 2 },
                new Enrollment { StudentID = 2, ProfessorID  = 2, Grade = 3 },
                new Enrollment { StudentID = 2, ProfessorID  = 3, Grade = 4 },
                new Enrollment { StudentID = 3, ProfessorID  = 1, Grade = 3 },
                new Enrollment { StudentID = 3, ProfessorID  = 2, Grade = 2 },
                new Enrollment { StudentID = 3, ProfessorID  = 3, Grade = 5 },
                new Enrollment { StudentID = 4, ProfessorID  = 1, Grade = 3 },
                new Enrollment { StudentID = 4, ProfessorID  = 2, Grade = 2 },
                new Enrollment { StudentID = 4, ProfessorID  = 3, Grade = 5 },
            };

            enrollments.ForEach(e => context.Enrollments.AddOrUpdate(e));
            context.SaveChanges();
        }
        

        void CreateRole(string roleName, RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExists(roleName))
                roleManager.Create(new ApplicationRole(roleName));
        }

        void CreateUser(string userName, string password, string roleName, UserManager<ApplicationUser> userManager)
        {
            var user = userManager.FindByName(userName);
            if (user == null)
            {
                var newUser = new ApplicationUser()
                {
                    UserName = userName
                };
                userManager.Create(newUser, password);
                userManager.SetLockoutEnabled(newUser.Id, false);
                userManager.AddToRole(newUser.Id, roleName);
            }
        }
    }
}

