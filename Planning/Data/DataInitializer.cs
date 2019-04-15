using Microsoft.AspNetCore.Identity;
using Repositories.Model;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningApi.Data
{
    public static class DataInitializer
    {
        public static void SeedData (UserManager<User> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext ctx)
        {
            SeedRoles(roleManager);
            //SeedUsers(userManager);
        }

        public static void SeedUsers  (UserManager<User> userManager)
        {
            if (userManager.FindByNameAsync("johnD").Result == null)
            {
                User user = new User() { Email = "johndoe@gmail.com", LastName = "john", FirstName = "doe", UserName = "johnD", EmailConfirmed = true };

                IdentityResult result = userManager.CreateAsync(user, "123456").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "USER").Wait();
                }
            }


            if (userManager.FindByNameAsync("janeD").Result == null)
            {
                User user = new User() { Email = "janedoe@gmail.com", LastName = "jane", FirstName = "doe", UserName = "janeD", EmailConfirmed = true };


                IdentityResult result = userManager.CreateAsync
                    (user, "123456").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "ADMIN").Wait();
                }
            }
        }

        public static void SeedRoles (RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync
            ("USER").Result)
            {
                ApplicationRole role = new ApplicationRole
                {
                    Name = "USER",
                    Description = "user that can do normal operations"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync
                ("ADMIN").Result)
            {
                ApplicationRole role = new ApplicationRole
                {
                    Name = "ADMIN",
                    Description = "Perform all the back office operations"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
