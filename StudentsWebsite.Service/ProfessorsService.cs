using StudentsWebsite.DAL;
using StudentsWebsite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsWebsite.Service
{
    public class ProfessorsService : PersonService
    {
        public ProfessorsService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            persons = (Repository<Profile>)unitOfWork.Professors;
        }

        public int NumberOfStudents(Professor professor)
        {
            return professor.Enrollments.Count;
        }

        public IEnumerable<Professor> WithLessStudents()
        {
            int minNum = persons.Get().Select(p => NumberOfStudents(p as Professor)).Min();

            return persons.Get().Where(p => NumberOfStudents(p as Professor) == minNum) as IEnumerable<Professor>;
        }

        public IEnumerable<Professor> WithAllStudents()
        {
            int numberOfStudents = unitOfWork.Students.Get().Count();

            return persons.Get().Where(p => p.Enrollments.Count == numberOfStudents) as IEnumerable<Professor>;
        }
    }
}
