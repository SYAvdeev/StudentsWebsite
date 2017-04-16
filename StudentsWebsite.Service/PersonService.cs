using StudentsWebsite.Domain.Entities;
using StudentsWebsite.DAL;
using StudentsWebsite.DAL.Interfaces;
using StudentsWebsite.Service.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace StudentsWebsite.Service
{
    public abstract class PersonService : IPersonService
    {
        protected UnitOfWork unitOfWork;
        protected Repository<Profile> persons;

        public PersonService(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public IEnumerable<Profile> GetAll()
        {
            return persons.Get();
        }

        public string FullName(Profile person)
        {
            return person.LastName + ", " + person.FirstName;
        }

        public IEnumerable<Profile> GetFiltered(
            Expression<Func<Profile, bool>> filter = null,
            Func<IQueryable<Profile>, IOrderedQueryable<Profile>> orderBy = null,
            string includeProperties = "")
        {
            return persons.Get(filter, orderBy, includeProperties);
        }

        public int Count()
        {
            return persons.Count();
        }
    }
}
