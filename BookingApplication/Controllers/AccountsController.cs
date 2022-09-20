using Contracts;
using Entities.DataTransferObjects.UserDtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BookingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
		private readonly UserManager<User> _userManager;
		private readonly ITokenRepository _tokenService;
		private readonly ILoggerManager _logger;

		public AccountsController(UserManager<User> userManager, ITokenRepository tokenService, ILoggerManager logger)
		{
			_userManager = userManager;
			_tokenService = tokenService;
			_logger = logger;
		}
		[AllowAnonymous]
		[HttpPost("Registration-patient")]
		public async Task<IActionResult> RegisterPatient([FromBody] UserForRegistrationDto userForRegistration)
		{
			if (userForRegistration == null || !ModelState.IsValid)
				return BadRequest();

			var user = new User { UserName = userForRegistration.Email, Email = userForRegistration.Email };
			var result = await _userManager.CreateAsync(user, userForRegistration.Password);
			if (!result.Succeeded)
			{
				var errors = result.Errors.Select(e => e.Description);

				return BadRequest(new RegistrationResponseDto { Errors = errors });
				_logger.LogError(errors.ToString());
			}

			await _userManager.AddToRoleAsync(user, "Admin");

			return StatusCode(201);
		}
        [Authorize(Roles = "Admin")]
		[HttpPost("Registration-doctor")]
		public async Task<IActionResult> RegisterDoctor([FromBody] UserForRegistrationDto userForRegistration)
		{
			if (userForRegistration == null || !ModelState.IsValid)
				return BadRequest();

			var user = new User { UserName = userForRegistration.Email, Email = userForRegistration.Email };
			var result = await _userManager.CreateAsync(user, userForRegistration.Password);
			if (!result.Succeeded)
			{
				var errors = result.Errors.Select(e => e.Description);

				return BadRequest(new RegistrationResponseDto { Errors = errors });
			}

			await _userManager.AddToRoleAsync(user, "Doctor");

			return StatusCode(201);
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
		{
			var user = await _userManager.FindByNameAsync(userForAuthentication.Email);

			if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
				return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

			var signingCredentials = _tokenService.GetSigningCredentials();
			var claims = await _tokenService.GetClaims(user);
			var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
			var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

			user.RefreshToken = _tokenService.GenerateRefreshToken();
			user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

			await _userManager.UpdateAsync(user);

			return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token, RefreshToken = user.RefreshToken });
		}
	}
}
