using Entities.DataTransferObjects.UserDtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUserRepository//:IRepositoryBase<User>
    {
        //Task<List<User>> GetAllUsersAsync();
        //Task<IList<string>> GetUserRolesAsync(User user);
        //Task<User> GetUserAsync(Guid id);
        //Task<IdentityResult> DeleteUserAsync(User user);
        //Task<IdentityResult> AddUserRolesAsync(User user, IList<string> rolesList);
        //Task<IdentityResult> RemoveUserRoleAsync(User user, IList<string> rolesList);
        Task<IdentityResult> UpdateUserAsync(User user, ChangeUserDto userToChange);
        Task<bool> UpdateUserRolesAsync(User user, ChangeUserRolesDto userToChange);


    }
}
