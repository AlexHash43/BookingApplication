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
    public static class SeedDefaultPatients
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            var defaultPatientsList = new List<User>();
            var defaultPatient1 = new User
            {
                UserName = "patient1",
                Email = "patient1@gmail.com",
                FirstName = "Andrew",
                LastName = "Maguire",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedOn = DateTime.Now
            };
            var defaultPatient2 = new User
            {
                UserName = "patient2",
                Email = "patient2@gmail.com",
                FirstName = "Linda",
                LastName = "Brown",
                //FirstName = "Michael",
                //LastName = "Armstrong",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedOn = DateTime.Now
            };
            defaultPatientsList.Add(defaultPatient1);
            defaultPatientsList.Add(defaultPatient2);
            //defaultPatientsList.ForEach(
            //    patient =>
            //    { 
            foreach (var patient in defaultPatientsList)
            {
                if (userManager.Users.All(u => u.Id != patient.Id))
                {
                    var user = await userManager.FindByEmailAsync(patient.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(patient, "Password123!");
                        await userManager.AddToRoleAsync(patient, Roles.Patient.ToString());
                    }
                }
            }
        }
    }
}
