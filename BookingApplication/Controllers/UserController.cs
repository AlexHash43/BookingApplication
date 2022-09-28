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
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> GetUsers()
            {
                var userDbList = await _userManager.Users.ToListAsync();

                if (userDbList is null) return BadRequest(AppResources.UsersDoNotExist);
            var userList = _mapper.Map<List<GetUserDto>>(userDbList);

            return Ok(userList);
            }

            [HttpGet("{id}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> GetDetailedUser(Guid id)
            {
                var userDb = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);

            //if (userDb is null) return BadRequest(AppResources.UsersDoNotExist);
                if (userDb is null) return NotFound(userDb);

                var roles = await _userManager.GetRolesAsync(userDb);
                return Ok(new GetUserDetailsDto
                {
                    Id = userDb.Id,
                    FirstName = userDb.FirstName,
                    LastName = userDb.LastName,
                    UserName = userDb.UserName,
                    //FullName = userDb.FullName,
                    Email = userDb.Email,
                    Address = userDb.Address,
                    PhoneNumber = userDb.PhoneNumber,
                    Roles = await _userManager.GetRolesAsync(userDb)
                });
                               
            }

            [HttpDelete]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            async public Task<IActionResult> DeleteUser(GetUserDto getUser)
            {
                if (getUser is null) return BadRequest(AppResources.NullUser);

                var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == getUser.Id);

                if (user is null) return BadRequest(AppResources.UserDeletionNoUserInDb);

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded) return BadRequest(AppResources.UserDeletionImpossible);

                var userDbList = _userManager.Users.ToList();

                if (userDbList is null) return Ok(new UserForRemovalDto { Message = AppResources.UserDeletionNoUsersLeft, User = null });

                var userReturnList = _mapper.Map<GetUserDto>(userDbList);

                return Ok(AppResources.UserDeleted);
            }

            [HttpPost("userupdate")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
