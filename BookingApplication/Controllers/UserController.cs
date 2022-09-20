using AutoMapper;
using BookingApplication.Resources;
using Contracts;
using Entities.DataTransferObjects.UserDtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookingApplication.Controllers
{
    [Authorize(Policy = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, UserManager<User> userManager)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }
    

            /// <summary>
            ///     Gets the user list from the database.
            /// </summary>
            /// <returns>The user list or bad request if there are no users.</returns>
            [HttpGet]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> GetUsers()
            {
                var userDbList = _userManager.Users.ToList();

                if (userDbList is null) return BadRequest(AppResources.UsersDoNotExist);

                var userList = userDbList.Select(user => new GetUserDto

                {
                    Id = user.Id,
                    FullName = $"{user.FirstName} {user.LastName}",
                    UserName = user.UserName,
                    Email = user.Email,
                });

                return Ok(userList);
            }

            /// <summary>
            ///     Gets a certain user from the database based on incoming <paramref name="id"/>.
            /// </summary>
            /// <returns>Returns the user or a bad request if there is no such user.</returns>
            [HttpGet("{id}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> GetDetailedUsers(Guid id)
            {
                var userDb = _userManager.Users.FirstOrDefault(user => user.Id == id);

            if (userDb is null) return BadRequest(AppResources.UsersDoNotExist);

                var roles = await _userManager.GetRolesAsync(userDb);

                return Ok(new GetUserDetailsDto
                {
                    Id = userDb.Id,
                    FirstName = userDb.FirstName,
                    LastName = userDb.LastName,
                    UserName = userDb.UserName,
                    Email = userDb.Email,
                    Address = userDb.Address,
                    PhoneNumber = userDb.PhoneNumber,
                    Roles = roles
                });
            }

            /// <summary>
            ///     Deletes the user provided by <paramref name="getUser"/>.
            ///     Uses <paramref name="cancellationToken"/> to cancel the procedure
            ///     on client request.
            /// </summary>
            /// <returns>Returns an Ok message or bad request if procedure unsuccessful.</returns>
            [HttpDelete]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            async public Task<IActionResult> DeleteUser(GetUserDto getUser)
            {
                if (getUser is null) return BadRequest(AppResources.NullUser);

                var user = _userManager.Users.FirstOrDefault(user => user.Id == getUser.Id);

                if (user is null) return BadRequest(AppResources.UserDeletionNoUserInDb);

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded) return BadRequest(AppResources.UserDeletionImpossible);

                var userDbList = _userManager.Users.ToList();

                if (userDbList is null) return Ok(new UserForRemovalDto { Message = AppResources.UserDeletionNoUsersLeft, User = null });

                var userReturnList = userDbList.Select(async user => new GetUserDto
                {
                    FullName = $"{user.FirstName} {user.LastName}",
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                });

                return Ok(AppResources.UserDeleted);
            }

            /// <summary>
            ///     Creates new user with incoming data from 
            ///     <paramref name="userToCreate"/>. Uses 
            ///     <paramref name="cancellationToken"/> to cancel
            ///     the procedure on client request.
            /// </summary>
            /// <returns>The updated list of users and an Ok message or a bad request if procedure unsuccessful.</returns>
            //[HttpPost("creation")]
            //[ProducesResponseType(StatusCodes.Status200OK)]
            //[ProducesResponseType(StatusCodes.Status400BadRequest)]
            //async public Task<IActionResult> CreateUser(UserForCreationDto userToCreate)
            //{
            //    if (userToCreate is null) return BadRequest(AppResources.NullUser);

            //    var user = _userManager.Users.FirstOrDefault(user => user.Email == userToCreate.Email);

            //    if (user is not null) return BadRequest(AppResources.UserAlreadyExists);

            //    var hasher = new PasswordHasher<UserForCreationDto>();
            //    var hash = hasher.HashPassword(userToCreate, userToCreate.Password);
            //    var newUser = new User
            //    {
            //        Email = userToCreate.Email,
            //        PasswordHash = hash,
            //        UserName = userToCreate.Email,
            //        FirstName = userToCreate.FirstName,
            //        LastName = userToCreate.LastName,
            //    };
            //    var result = await _userManager.CreateAsync(newUser);

            //    if (!result.Succeeded) return BadRequest(AppResources.UserCreationImpossible);
            //    else
            //    {
            //        var roleResult = await _userManager.AddToRolesAsync(newUser, userToCreate.Roles);
            //        var userDbList = _userManager.Users.ToList();
            //        var userReturnList = userDbList.Select(async user => new GetUserDto
            //        {
            //            Id = user.Id,
            //            FullName = $"{user.FirstName} {user.LastName}",
            //            UserName = user.UserName,
            //            Email = user.Email,
            //            Roles = await _userManager.GetRolesAsync(user)
            //        });

            //        return Ok(new { Users = userReturnList, Message = AppResources.UserCreated });
            //    }
            //}

            /// <summary>
            ///     Modifies an existing user with data from 
            ///     <paramref name="userToChange"/>. Uses 
            ///     <paramref name="cancellationToken"/> to cancel the procedure
            ///     on user request.
            /// </summary>
            /// <returns>The updated user list and an Ok message or bad request if unsuccessful.</returns>
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
                    && userRoles == userToChange.Roles
                ) return BadRequest(AppResources.UserModificationSameData);

                if (userToChange.Email is not null) user.Email = userToChange.Email;
                if (userToChange.UserName is not null) user.UserName = userToChange.UserName;
                if (userToChange.FirstName is not null) user.FirstName = userToChange.FirstName;
                if (userToChange.LastName is not null) user.LastName = userToChange.LastName;

                var rolesNotAdded = new List<string>();

                if (userToChange.Roles.Any())
                {
                    var rolesRemovalResult = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                    var roleResult = await _userManager.AddToRolesAsync(user, userToChange.Roles);
                }

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded) return BadRequest(AppResources.UserEditImpossible);
                else
                {
                    var userDbList = _userManager.Users.ToList();
                    var userReturnList = userDbList.Select(async user => new GetUserDto
                    {
                        Id = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        UserName = user.UserName,
                        Email = user.Email,
                        Roles = await _userManager.GetRolesAsync(user)
                    });

                    if (!rolesNotAdded.Any()) return Ok(new { Users = userReturnList, Message = AppResources.UserEdited });

                    return Ok(new { Users = userReturnList, Message = string.Format(AppResources.UserEditedNoRoles, string.Join(",", rolesNotAdded.ToArray())) });
                }
            }
    }


}
