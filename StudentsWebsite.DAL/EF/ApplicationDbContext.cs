using Microsoft.AspNet.Identity.EntityFramework;
using StudentsWebsite.DAL.Entities;
using System.Data.Entity;

namespace StudentsWebsite.DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        ApplicationDbContext(string connectionString) : base(connectionString) { }

    }
}
