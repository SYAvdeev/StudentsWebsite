using StudentsWebsite.DAL;
using StudentsWebsite.DAL.Interfaces;
using StudentsWebsite.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace StudentsWebsite.Service
{
    public class StudentsService : PersonService
    {
        public StudentsService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            persons = (Repository<Profile>)unitOfWork.Students;
        }

        public float AverageGrade(Student student)
        {
            if (student.Enrollments == null)
                return 0f;
            if (student.Enrollments.Count == 0)
                return 0f;
            else
                return (float)student.Enrollments.Average(e => e.Grade ?? 0);
        }

        public IEnumerable<Student> StatisticsHighestGrade()
        {
            double allAverage = unitOfWork.Enrollments.Get().Average(e => e.Grade ?? 0);

            return persons.Get().Where(p => AverageGrade(p as Student) >= allAverage) as IEnumerable<Student>;
        }
    }
}
