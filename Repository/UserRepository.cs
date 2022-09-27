using AutoMapper;
using Contracts;
using Entities;
using Entities.DataTransferObjects.UserDtos;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public UserRepository(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, UserManager<User> userManager)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }

        //public async Task<IdentityResult> DeleteUserAsync(User user)
        //{
        //    return await _userManager.DeleteAsync(user);
        //}

        //public async Task<User> GetUserAsync(Guid id)
        //{
        //    return await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
        //}

        //public async Task<List<User>> GetAllUsersAsync()
        //{
        //    return await _userManager.Users.ToListAsync();
        //}

        //public async Task<IList<string>> GetUserRolesAsync(User user)
        //{
        //    return await _userManager.GetRolesAsync(user);
        //}
        public async Task<IdentityResult> UpdateUserAsync(User user, ChangeUserDto userToChange)
        {
            user.Email = userToChange.Email is not null ? userToChange.Email : user.Email;
            user.UserName = userToChange.UserName is not null ? userToChange.UserName : user.UserName;
            user.FirstName = userToChange.FirstName is not null ? userToChange.FirstName : user.FirstName;
            user.LastName = userToChange.LastName is not null ? userToChange.LastName : user.LastName;

            //var rolesNotAdded = new List<string>();

            //bool userRolesUpdateResult = await UpdateUserRolesAsync(user, userToChange);
            
            return await _userManager.UpdateAsync(user);
        }

        //public async Task<IdentityResult> AddUserRolesAsync(User user, IList<string> rolesList)
        //{
        //    return await _userManager.AddToRolesAsync(user, rolesList);
        //}

        //public async Task<IdentityResult> RemoveUserRoleAsync(User user, IList<string> rolesList)
        //{
        //    return await _userManager.RemoveFromRolesAsync(user, rolesList);

        //}

        public async Task<bool> UpdateUserRolesAsync(User user, ChangeUserRolesDto userToChange)
        {
            bool result = false;
            if (userToChange.Roles.Any())
            {
                var rolesRemovalResult = await _userManager.AddToRolesAsync(user, await _userManager.GetRolesAsync(user));
                var roleResult = await _userManager.AddToRolesAsync(user, userToChange.Roles);
                result = rolesRemovalResult.Succeeded && roleResult.Succeeded? true : false;
                return result;
            }
            else
            {
                var rolesRemovalResult = await _userManager.RemoveFromRolesAsync(user, (IEnumerable<string>)_userManager.GetRolesAsync(user));
                result = rolesRemovalResult.Succeeded;
                return result;
            }          
        }

        //private readonly IHttpContextAccessor _httpContextAccessor;

        //public UserRepository(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}

        //public string GetUserName()
        //{
        //    var result = string.Empty;
        //    if (_httpContextAccessor.HttpContext != null)
        //    {
        //        result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        //    }
        //    return result;
        //}
        //public string GetUserRoles()
        //{
        //    var username = GetUserName();
        //    var result = string.Empty;
        //    if (_httpContextAccessor.HttpContext != null)
        //    {
        //        result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        //    }
        //    return result;
        //}
    }
}
