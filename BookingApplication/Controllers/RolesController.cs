using BookingApplication.Resources;
using Contracts;
using Entities;
using Entities.DataTransferObjects.RoleDtos;
using Entities.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingApplication.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        //private readonly AppointmentContext _context;
        private readonly ILoggerManager _logger;

        public RolesController(RoleManager<IdentityRole<Guid>> roleManager, AppointmentContext context, ILoggerManager loggerManager )
        {
            _roleManager = roleManager;
            //_context = context;
            _logger = loggerManager;
        }

        // GET: api/<RolesController>
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roleDbList = await _roleManager.Roles.ToListAsync();
            if (!roleDbList.Any()) return BadRequest(AppResources.RolesDoNotExist);
            var roleList = roleDbList.Select(role => new RoleForReturn
            {
                Id = role.Id,
                RoleName = role.Name
            });

            return Ok(roleList);
        }

        // GET api/<RolesController>/5
        [HttpGet("rolid")]
        public async Task<IActionResult> GetRole(Guid id)
        {
            var userRoles = await _roleManager.Roles.Where(role => role.Id == id).ToListAsync();
            if (!userRoles.Any()) return BadRequest(AppResources.RolesDoNotExist);
            var roleDetails = userRoles.Select(role => new RoleForReturn
            {
                Id = role.Id,
                RoleName = role.Name
            });

            return Ok(roleDetails);
        }

        // POST api/<RolesController>
        [HttpPost("createrole")]
        public async Task<IActionResult> CreateRoleAsync(string roleName)
        {
            if (roleName == string.Empty) return BadRequest(AppResources.NullRole);

            var checkRoleIfExists = await _roleManager.RoleExistsAsync(roleName.ToString());
            if (!checkRoleIfExists)
            {
                var newRole = new IdentityRole<Guid>
                {
                    Name = roleName
                };
                var result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    var roleDbList = _roleManager.Roles.ToList();
                    var roleList = roleDbList.Select(role => new RoleForReturn
                    {
                        Id = role.Id,
                        RoleName = role.Name
                    });

                    return Ok(new { Roles = roleList, Message = AppResources.RoleCreated });
                }
                return BadRequest(AppResources.RoleCreationImpossible);
            }
            return BadRequest(AppResources.RoleAlreadyExists);
        }

        // PUT api/<RolesController>/5
        [HttpPut]//("{updaterole}")]
        public async Task<IActionResult> EditRoleASync(RoleForReturn role)
        {
            if (role == null) return NotFound();
            var originalRole = await _roleManager.Roles.Where(a => a.Id == role.Id).FirstOrDefaultAsync();
            if (originalRole == null) return NotFound();
            originalRole.Name = role.RoleName;
            //_context.Roles.Update(originalRole);
            var updater = await _roleManager.UpdateAsync(originalRole);
            //var saver = await _context.SaveChangesAsync();
            if (!updater.Succeeded) //|| saver==0)
                return BadRequest();

            return Ok(GetRoles());
        }

        // DELETE api/<RolesController>/5
        [HttpDelete]//("{id}")]
        public async Task<IActionResult> DeleteRoleAsync(RoleForReturn roleModel)
        {
            if (roleModel == null) return BadRequest(AppResources.NullRole);
            var roleExists = await _roleManager.RoleExistsAsync(roleModel.RoleName);
            //var role = await _context.Roles.FirstOrDefaultAsync(role => role.Id == roleModel.Id);
            if (roleExists)
            {
                var role = await _roleManager.FindByNameAsync(roleModel.RoleName);
                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded) return BadRequest(AppResources.RoleDeletionImpossible);
                return Ok(GetRoles());
            }
            return BadRequest(AppResources.RoleDoesNotExist);

        }

        [HttpPost("createDefaultRoles")]
        public async Task<IActionResult> CreateDefaultRolesAsync()//this Roles roles )
        {
            List<IdentityRole<Guid>> newRoleList = new List<IdentityRole<Guid>>();
            foreach (Roles roleName in Enum.GetValues(typeof(Roles)))
            {
                var checkRoleIfExists = await _roleManager.RoleExistsAsync(roleName.ToString());
                if (!checkRoleIfExists)
                {
                    var newRole = new IdentityRole<Guid>
                    {
                        Name = roleName.ToString(),
                    };
                    var result = await _roleManager.CreateAsync(newRole);

                    if (result.Succeeded)
                    {
                        newRoleList.Add(newRole);
                    }
                }               
            }
            //_context.SaveChangesAsync();
            if(newRoleList.Any())
            return Ok(new { Roles = newRoleList, Message = AppResources.RoleCreated });
            return BadRequest(AppResources.RoleCreationImpossible);

        }
    }
}
