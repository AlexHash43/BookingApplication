using Contracts;
using Entities;
using Entities.DataTransferObjects.UserDtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookingApplication.Resources;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Entities.DataTransferObjects.UserDtos.UserToken.UserAuth;
using Entities.DataTransferObjects.UserDtos.UserAuth;

namespace BookingApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
		private readonly UserManager<User> _userManager;
		private readonly ITokenRepository _authService;

		public AuthController(UserManager<User> userManager, ITokenRepository authService)
		{
			_userManager = userManager;
			_authService = authService;
		}

		[HttpPost]
		[Route("refresh")]
		public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto tokenDto)
		{
			if (tokenDto is null)
			{
				return BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });
			}

			var principal = _authService.GetPrincipalFromExpiredToken(tokenDto.Token);
			var username = principal.Identity.Name;

			var user = await _userManager.FindByEmailAsync(username);
			if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
				return BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });

			var signingCredentials = _authService.GetSigningCredentials();
			var claims = await _authService.GetClaims(user);
			var tokenOptions = _authService.GenerateTokenOptions(signingCredentials, claims);
			var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
			user.RefreshToken = _authService.GenerateRefreshToken();

			await _userManager.UpdateAsync(user);

			return Ok(new AuthResponseDto { Token = token, RefreshToken = user.RefreshToken, IsAuthSuccessful = true });
		}
	}

}
