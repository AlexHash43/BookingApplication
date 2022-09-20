using BookingApplication.Resources;
using Entities;
using Entities.DataTransferObjects.RoleDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly AppointmentContext _context;
        public RolesController(RoleManager<IdentityRole<Guid>> roleManager, AppointmentContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/<RolesController>
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost("createrole")]
        public async Task<IActionResult> CreateRoleAsync(string roleName)
        {
            if (roleName == string.Empty) return BadRequest(AppResources.NullRole);

            var role = await _context.Roles.Where(r => r.Name == roleName).FirstOrDefaultAsync();
            if (role is not null) return BadRequest(AppResources.RoleAlreadyExists);

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

        // PUT api/<RolesController>/5
        [Authorize(Roles = "Admin")]
        [HttpPut]//("{updaterole}")]
        public async Task<IActionResult> EditRoleASync(RoleForReturn role)//, CancellationToken cancellationToken)
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
        [Authorize(Roles = "Admin")]
        [HttpDelete]//("{id}")]
        public async Task<IActionResult> DeleteRoleAsync(RoleForReturn roleModel)
        {
            if (roleModel == null) return BadRequest(AppResources.NullRole);
            var role = await _context.Roles.FirstOrDefaultAsync(role => role.Id == roleModel.Id);
            if (role == null) return BadRequest(AppResources.RoleDoesNotExist);
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded) return BadRequest(AppResources.RoleDeletionImpossible);
            return Ok(GetRoles());

        }
    }
}
