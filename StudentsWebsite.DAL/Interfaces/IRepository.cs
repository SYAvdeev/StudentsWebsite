using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace StudentsWebsite.DAL.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        T GetByID(int id);

        void Insert(T entity);

        void Delete(int id);

        void Delete(T entityToDelete);

        void Update(T entityToUpdate);
    }
}
