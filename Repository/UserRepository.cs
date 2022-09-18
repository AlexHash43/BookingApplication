using Contracts;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppointmentContext appointmentContext) : base(appointmentContext)
        {
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
