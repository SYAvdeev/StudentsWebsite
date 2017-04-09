using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsWebsite.DAL.Entities
{
    public class Student : Profile
    {
        public float AverageGrade
        {
            get
            {
                if (Enrollments == null)
                    return 0f;
                if (Enrollments.Count == 0)
                    return 0f;
                else
                    return (float)Enrollments.Average(e => e.Grade ?? 0);
            }
        }
    }
}
