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
            try
            {
                var roleDbList = await _roleManager.Roles.ToListAsync();
                if (roleDbList.Any())
                {
                    var roleList = roleDbList.Select(role => new RoleForReturn
                    {
                        Id = role.Id,
                        RoleName = role.Name
                    });
                    _logger.LogInfo("Returned all roles from Database");
                    return Ok(roleList);
                }

                else
                {
                    _logger.LogInfo("No roles in the Database");
                    return BadRequest(AppResources.RolesDoNotExist);
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetRoles Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET api/<RolesController>/5
        [HttpGet("rolid:Guid")]
        public async Task<IActionResult> GetRole(Guid id)
        {
            try
            {
                var roleDb = await _roleManager.Roles.Where(role => role.Id == id).FirstOrDefaultAsync();
                if (roleDb != null)
                {
                    var roleDetails = new RoleForReturn
                    {
                        Id = roleDb.Id,
                        RoleName = roleDb.Name
                    };
                    _logger.LogInfo("Returned all roles from Database");
                    return Ok(roleDetails);
                }

                else
                {
                    _logger.LogInfo("No roles in the Database");
                    return BadRequest(AppResources.RolesDoNotExist);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetRoles Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST api/<RolesController>
        [HttpPost("createrole")]
        public async Task<IActionResult> CreateRoleAsync(string roleName)
        {
            try
            { 
                if (roleName == string.Empty)
                {
                    _logger.LogError("Rolename sent from clioent is null");
                    return BadRequest(AppResources.NullRole);
                }
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
                    _logger.LogInfo($"Creating role with name {roleName} failed");
                    return BadRequest(AppResources.RoleCreationImpossible);
                }
                _logger.LogInfo($"Role with name: {roleName} already exists");
                return BadRequest(AppResources.RoleAlreadyExists);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Creating role Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }

        // PUT api/<RolesController>/5
        [HttpPut]//("{updaterole}")]
        public async Task<IActionResult> UpdateRoleAsync(RoleForReturn role)
        {
            try
            {
                if (role == null)
                {
                    _logger.LogError("Role object sent from the client is null");
                    return BadRequest("Role object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid role object sent from the client");
                    return BadRequest("Invalid model object");
                }
                var originalRole = await _roleManager.Roles.Where(a => a.Id == role.Id).FirstOrDefaultAsync();
                if (originalRole != null)
                {
                    originalRole.Name = role.RoleName;
                    var updater = await _roleManager.UpdateAsync(originalRole);
                    if (updater.Succeeded)
                    {
                        _logger.LogInfo("Returned updated role");
                        return Ok(GetRole(role.Id));
                    }
                    _logger.LogInfo($"Updating role with Id {role.Id} failed");
                    return BadRequest($"Updating role failed");

                }
                _logger.LogInfo($"Role with Id {role.Id} was not found in the database");
                return NotFound("No such role in the database");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Updating role {role.RoleName} in the Database failed" );
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }

        }

        // DELETE api/<RolesController>/5
        [HttpDelete]//("{id}")]
        public async Task<IActionResult> DeleteRoleAsync(string id)
        {
            try
            {
                var roleEntity = await _roleManager.FindByIdAsync(id);

                if (roleEntity != null)
                {
                    var result = await _roleManager.DeleteAsync(roleEntity);
                    if (result.Succeeded)
                    {
                        return Ok(GetRoles());
                    }
                    return BadRequest(AppResources.RoleDeletionImpossible);
                }
                return BadRequest(AppResources.RoleDoesNotExist);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete role Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
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
