using StudentsWebsite.Domain.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsWebsite.Domain.Entities
{
    public abstract class Profile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public int ID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }

    }
}

