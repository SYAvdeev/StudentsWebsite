using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using StudentsWebsite.DAL.Interfaces;
using StudentsWebsite.Domain.Entities;
using StudentsWebsite.Domain.Identity;
using System;

namespace StudentsWebsite.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private ApplicationDbContext context;

        IRepository<Student> studentsRepository;
        IRepository<Professor> professorsRepository;
        IRepository<Enrollment> enrollmentsRepository;

        public IRepository<Student> Students { get { return studentsRepository; } }
        public IRepository<Professor> Professors { get { return professorsRepository; } }
        public IRepository<Enrollment> Enrollments { get { return enrollmentsRepository; } }

        ApplicationUserManager userManager;
        ApplicationRoleManager roleManager;
        ApplicationSignInManager signInManager;

        public ApplicationUserManager UserManager { get { return userManager; } }
        public ApplicationRoleManager RoleManager { get { return roleManager; } }
        public ApplicationSignInManager SignInManager { get { return signInManager; } }

        public UnitOfWork(string connectionString, IOwinContext owinContext)
        {
            context = new ApplicationDbContext(connectionString);

            studentsRepository = new Repository<Student>(context);
            enrollmentsRepository = new Repository<Enrollment>(context);
            professorsRepository = new Repository<Professor>(context);

            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
            
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
            };

            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));
            signInManager = new ApplicationSignInManager(userManager, owinContext.Authentication);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    signInManager.Dispose();
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
