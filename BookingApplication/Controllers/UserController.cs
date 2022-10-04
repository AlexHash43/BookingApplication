using AutoMapper;
using BookingApplication.Resources;
using Contracts;
using Entities.DataTransferObjects.UserDtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingApplication.Controllers
{

    //Implement Logging, try/catch ex()
    //[Authorize(Policy = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public UserController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, UserManager<User> userManager,  IUserRepository userRepository)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _userRepository = userRepository;
        }

            [HttpGet]
            public async Task<IActionResult> GetUsers()
            {
                try
                {   
                    var userDbList = await _userManager.Users.ToListAsync();
                    if (userDbList.Any())
                    {
                        _logger.LogInfo("Returned all users from Database");
                        var userList = _mapper.Map<List<GetUserDto>>(userDbList);
                        return Ok(userList);
                    }
                    else
                    {
                        _logger.LogInfo("No users in the Database");
                        return BadRequest();
                    }
                                        
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetUsers Failed: {ex.Message}");
                    return StatusCode(500, $"Internal Server Error: {ex.Message}");
                }
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetDetailedUser(Guid id)
            {
            try
            {
                var userDb = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);

                if (userDb != null)
                {
                    _logger.LogInfo($"Returned user with id: {id} from Database");
                    return Ok(new GetUserDetailsDto
                    {
                        Id = userDb.Id,
                        FirstName = userDb.FirstName,
                        LastName = userDb.LastName,
                        UserName = userDb.UserName,
                        Email = userDb.Email,
                        Address = userDb.Address,
                        PhoneNumber = userDb.PhoneNumber,
                        Roles = await _userManager.GetRolesAsync(userDb)
                    });
                }
                else
                {
                    _logger.LogInfo($"No user with id: {id} was found in the Database");
                    return NotFound();
                }
            }
                catch (Exception ex)
                {
                    _logger.LogError($"GetDetailedUser Failed: {ex.Message}");
                    return StatusCode(500, $"Internal Server Error:{ex.Message}");
                }
            }

            [HttpDelete("{id}")]
            async public Task<IActionResult> DeleteUser(GetUserDto getUser)
            {
                try
                {
                    if (getUser != null)
                    {
                        var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == getUser.Id);
                        if (user != null)
                        {
                            _logger.LogInfo($"User with id {getUser.Id} found in the DB");
                            var result = await _userManager.DeleteAsync(user);
                            if (result.Succeeded)
                            {
                            _logger.LogInfo($"User with id {getUser.Id} deleted from the DB");
                                var userDbList = _userManager.Users.ToList();

                                if (userDbList is null) return Ok(new UserForRemovalDto { Message = AppResources.UserDeletionNoUsersLeft, User = null });

                                var userReturnList = _mapper.Map<GetUserDto>(userDbList);
                                
                                return Ok(AppResources.UserDeleted);
                            }
                            return BadRequest(AppResources.UserDeletionImpossible);
                        }
                        _logger.LogError($"User with id: {getUser.Id}, hasn't been found in the DB.");
                        return NotFound(AppResources.UserDeletionNoUserInDb);
                    }
                    else
                    {
                        _logger.LogInfo("Provided user object for deletion is null");
                        return BadRequest(AppResources.NullUser);
                    }
                                     
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Deleting user Failed: {ex.Message}");
                    return StatusCode(500, $"Internal Server Error:{ex.Message}");
                }

            }

            [HttpPost("userupdate")]

            public async Task<IActionResult> ChangeUserAsync(ChangeUserDto userToChange)
            {
                if (userToChange is null) return BadRequest(AppResources.NullUser);

                var user = _userManager.Users.FirstOrDefault(user => user.Id == userToChange.Id);

                if (user is null) return BadRequest(AppResources.UserDoesNotExist);

                var userRoles = await _userManager.GetRolesAsync(user);

                if
                (
                    user.Email == userToChange.Email
                    && user.UserName == userToChange.UserName
                    && user.FirstName == userToChange.FirstName
                    && user.LastName == userToChange.LastName
                ) return BadRequest(AppResources.UserModificationSameData);


               var result = await _userRepository.UpdateUserAsync(user, userToChange);

                if (!result.Succeeded) return BadRequest(AppResources.UserEditImpossible);
                else
                {
                    var userDbList = _userManager.Users.ToList();
                    var userReturnList = _mapper.Map<GetUserDto>(userDbList);

                return Ok(new { Users = userReturnList, Message = "User updated with success" });
                }
            }
    }


}
