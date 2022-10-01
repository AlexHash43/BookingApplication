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
    public class Seed
    {
        public static async Task SeedData(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            await SeedDefaultRoles.SeedAsync(userManager, roleManager);
            await SeedDefaultSuperAdmin.SeedAsync(userManager, roleManager);
            await SeedDefaultAdmin.SeedAsync(userManager, roleManager);
            await SeedDefaultDoctors.SeedAsync(userManager, roleManager);
            await SeedDefaultPatients.SeedAsync(userManager, roleManager);

        }
        //    //Seed Roles
        //    await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.SuperAdmin.ToString()));
        //    await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Admin.ToString()));
        //    await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Doctor.ToString()));
        //    await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Patient.ToString()));

        //    //Seed SuperAdmin
        //    var superAdminUser = new User
        //    {
        //        UserName = "superadmin",
        //        Email = "superadmin@gmail.com",
        //        FirstName = "Alex",
        //        LastName = "Gisca",
        //        EmailConfirmed = true,
        //        PhoneNumberConfirmed = true
        //    };
        //    if (userManager.Users.All(u => u.Id != superAdminUser.Id))
        //    {
        //        var user = await userManager.FindByEmailAsync(superAdminUser.Email);
        //        if (user == null)
        //        {
        //            await userManager.CreateAsync(superAdminUser, "123Pa$$word!");
        //            await userManager.AddToRoleAsync(superAdminUser, Roles.Patient.ToString());
        //            await userManager.AddToRoleAsync(superAdminUser, Roles.Doctor.ToString());
        //            await userManager.AddToRoleAsync(superAdminUser, Roles.Admin.ToString());
        //            await userManager.AddToRoleAsync(superAdminUser, Roles.SuperAdmin.ToString());
        //        }

        //    }
        //    //Seed SuperAdmin
        //    var adminUser = new User
        //    {
        //        UserName = "admin",
        //        Email = "admin@gmail.com",
        //        FirstName = "Admin",
        //        LastName = "Admin",
        //        EmailConfirmed = true,
        //        PhoneNumberConfirmed = true
        //    };
        //    if (userManager.Users.All(u => u.Id != adminUser.Id))
        //    {
        //        var user = await userManager.FindByEmailAsync(adminUser.Email);
        //        if (user == null)
        //        {
        //            await userManager.CreateAsync(adminUser, "123Pa$$word!");
        //            await userManager.AddToRoleAsync(adminUser, Roles.Patient.ToString());
        //            await userManager.AddToRoleAsync(adminUser, Roles.Doctor.ToString());
        //            await userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
        //        }
        //    }

        //    var defaultDoctorsList = new List<User>();
        //    var defaultDoctor1 = new User
        //    {
        //        UserName = "doctor1",
        //        Email = "doctor1@gmail.com",
        //        FirstName = "Maria",
        //        LastName = "Rodriguez",
        //        EmailConfirmed = true,
        //        PhoneNumberConfirmed = true,
        //        CreatedOn = DateTime.Now
        //    };
        //    var defaultDoctor2 = new User
        //    {
        //        UserName = "doctor2",
        //        Email = "doctor1@gmail.com",
        //        FirstName = "Angelo",
        //        LastName = "Smith",
        //        EmailConfirmed = true,
        //        PhoneNumberConfirmed = true,
        //        CreatedOn = DateTime.Now
        //    };
        //    defaultDoctorsList.Add(defaultDoctor1);
        //    defaultDoctorsList.Add(defaultDoctor2); 
        //    foreach (var patient in defaultDoctorsList)
        //    {
        //        if (userManager.Users.All(u => u.Id != patient.Id))
        //        {
        //            var user = await userManager.FindByEmailAsync(patient.Email);
        //            if (user == null)
        //            {
        //                await userManager.CreateAsync(patient, "Pa$$word123!");
        //                await userManager.AddToRoleAsync(patient, Roles.Doctor.ToString());
        //            }
        //        }
        //    }

        //    var defaultPatientsList = new List<User>();
        //    var defaultPatient1 = new User
        //    {
        //        UserName = "patient1",
        //        Email = "patient1@gmail.com",
        //        FirstName = "Andrew",
        //        LastName = "Maguire",
        //        EmailConfirmed = true,
        //        PhoneNumberConfirmed = true,
        //        CreatedOn = DateTime.Now
        //    };
        //    var defaultPatient2 = new User
        //    {
        //        UserName = "patient2",
        //        Email = "patient2@gmail.com",
        //        FirstName = "Linda",
        //        LastName = "Brown",
        //        //FirstName = "Michael",
        //        //LastName = "Armstrong",
        //        EmailConfirmed = true,
        //        PhoneNumberConfirmed = true,
        //        CreatedOn = DateTime.Now
        //    };
        //    defaultPatientsList.Add(defaultPatient1);
        //    defaultPatientsList.Add(defaultPatient2);
        //    //defaultPatientsList.ForEach(
        //    //    patient =>
        //    //    { 
        //    foreach (var patient in defaultPatientsList)
        //    {
        //        if (userManager.Users.All(u => u.Id != patient.Id))
        //        {
        //            var user = await userManager.FindByEmailAsync(patient.Email);
        //            if (user == null)
        //            {
        //                await userManager.CreateAsync(patient, "Password123!");
        //                await userManager.AddToRoleAsync(patient, Roles.Patient.ToString());
        //            }
        //        }
        //    }
        //}

    }
}
