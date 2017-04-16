using StudentsWebsite.DAL;
using StudentsWebsite.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StudentsWebsite.Service
{
    public class IdentityService
    {
        UnitOfWork unitOfWork;

        public async Task<OperationDetails> Create(UserInfo userInfo)
        {
            ApplicationUser user = await unitOfWork.UserManager.FindByNameAsync(userInfo.Login);
            if (user == null)
            {
                user = new ApplicationUser { Email = userInfo.Login };
                var result = await unitOfWork.UserManager.CreateAsync(user, userInfo.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                
                await unitOfWork.UserManager.AddToRoleAsync(user.Id, userInfo.Role);
                
                 unitOfWork.Save();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }
    }
}
