using Microsoft.AspNet.Identity.EntityFramework;
using StudentsWebsite.Domain.Entities;
using StudentsWebsite.Domain.Identity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace StudentsWebsite.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        public ApplicationDbContext(string connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
