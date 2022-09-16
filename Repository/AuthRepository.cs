using Contracts;
using Entities;
using Entities.DataTransferObjects.UserDtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AuthRepository : IAuthRepository
    {
        private string _privateKey;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly AppointmentContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthRepository(string privateKey,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            AppointmentContext context,
            RoleManager<IdentityRole> roleManager)
        {
            _privateKey = privateKey;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;

        }

        public string Authentication(UserAuthDto authUser, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, authUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role)

                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF32.GetBytes(_privateKey)),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            //Check
            tokenHandler.WriteToken(token);
            return tokenHandler.WriteToken(token);

        }


    }
}
