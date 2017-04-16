using StudentsWebsite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsWebsite.DAL.Interfaces
{
    public interface IProfileManager : IDisposable
    {
        void Create(Profile item);
    }
}
