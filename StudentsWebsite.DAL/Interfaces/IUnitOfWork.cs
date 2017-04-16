using StudentsWebsite.Domain.Entities;
using System;

namespace StudentsWebsite.DAL.Interfaces
{
    public class IUnitOfWork
    {
        IRepository<Student> Students { get; }
        IRepository<Professor> Professors { get; }
        IRepository<Enrollment> Enrollments { get; }

        ApplicationDbContext Context { get; }
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        ApplicationSignInManager SignInManager { get; }
    }
}
