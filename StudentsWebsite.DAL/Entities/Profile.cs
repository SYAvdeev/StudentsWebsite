
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsWebsite.DAL.Entities
{
    public abstract class Profile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public int ID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get { return LastName + ", " + FirstName; } }
        
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }

    }
}

