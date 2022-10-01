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
    public static class SeedDefaultDoctors
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            var defaultDoctorsList = new List<User>();
            var defaultDoctor1 = new User
            {
                UserName = "doctor1",
                Email = "doctor1@gmail.com",
                FirstName = "Maria",
                LastName = "Rodriguez",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedOn = DateTime.Now
            };
            var defaultDoctor2 = new User
            {
                UserName = "doctor2",
                Email = "doctor1@gmail.com",
                FirstName = "Angelo",
                LastName = "Smith",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedOn = DateTime.Now
            };
            defaultDoctorsList.Add(defaultDoctor1);
            defaultDoctorsList.Add(defaultDoctor2);
            //defaultPatientsList.ForEach(
            //    patient =>
            //    { 
            foreach (var patient in defaultDoctorsList)
            {
                if (userManager.Users.All(u => u.Id != patient.Id))
                {
                    var user = await userManager.FindByEmailAsync(patient.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(patient, "Pa$$word123!");
                        await userManager.AddToRoleAsync(patient, Roles.Doctor.ToString());
                    }
                }
            }


        }
    }
}
