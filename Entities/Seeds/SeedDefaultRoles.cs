using Entities.Models;
using Entities.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Seeds
{
    public static class SeedDefaultRoles
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Doctor.ToString()));
            await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Patient.ToString()));
        }
    }
}
