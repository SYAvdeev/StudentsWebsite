using StudentsWebsite.Domain.Entities;
using StudentsWebsite.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace StudentsWebsite.Service.Interface
{
    interface IPersonService
    {
        IEnumerable<Profile> GetAll();

        IEnumerable<Profile> GetFiltered(
            Expression<Func<Profile, bool>> filter = null,
            Func<IQueryable<Profile>, IOrderedQueryable<Profile>> orderBy = null,
            string includeProperties = "");

        int Count();
    }
}
