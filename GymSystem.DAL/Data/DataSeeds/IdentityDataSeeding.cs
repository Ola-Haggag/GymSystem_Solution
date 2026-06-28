using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.DataSeeds
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,ILogger logger
            ,CancellationToken ct = default)
        {
            try
            {
                bool HasUsers = userManager.Users.Any();
                bool HasRoles = roleManager.Roles.Any();
                if (HasUsers && HasRoles) return;

                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new IdentityRole() {Name = "SuperAdmin"},
                        new IdentityRole() {Name = "Admin"},

                    };
                    foreach(var roleName in Roles.Select(R => R.Name))
                    {
                        if(!await roleManager.RoleExistsAsync(roleName))
                        {
                            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));

                            if (!roleResult.Succeeded)
                                logger.LogError("Failed to create Role..");
                        }
                    }
                }

                if (!HasUsers)
                {
                    var MainUser = new ApplicationUser()
                    {
                        FirstName = "Ola",
                        LastName = "Haggag",
                        UserName = "OlaH",
                        Email = "ola@gmail.com",
                        PhoneNumber = "01545440223",
                    };
                    var UserResult = await userManager.CreateAsync(MainUser, "P@ss0rd");
                    await userManager.AddToRoleAsync(MainUser, "SuperAdmin");

                    if (!UserResult.Succeeded)
                    {
                        logger.LogError("Failed to seed User");
                        return;
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to seed Identity Data");
                throw;
            }
        }
    }
}
