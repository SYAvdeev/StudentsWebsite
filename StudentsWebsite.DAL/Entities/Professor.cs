using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsWebsite.DAL.Entities
{
    public class Professor : Profile
    {
        public string Course { get; set; }
        public int NumOfStudents { get { return Enrollments.Count; } }
    }
}
