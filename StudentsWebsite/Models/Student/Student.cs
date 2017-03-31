using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsWebsite.Models
{
    public class Student : Person
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