using BookStore.Domain.Models.Entities.Membership;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.DataContexts
{
    static public class BookStoreDbSeed
    {
        public static IApplicationBuilder SeedMembership(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<BookStoreUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<BookStoreRole>>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                string superAdminRoleName = configuration["defaultAccount:superAdmin"];
                string superAdminEmail = configuration["defaultAccount:email"];
                string superAdminUserName = configuration["defaultAccount:username"];
                string superAdminPassword = configuration["defaultAccount:password"];


                var superAdminRole = roleManager.FindByNameAsync(superAdminRoleName).Result;

                if (superAdminRole == null)
                {
                    superAdminRole = new BookStoreRole
                    {
                        Name = superAdminRoleName
                    };


                    var roleResult = roleManager.CreateAsync(superAdminRole).Result;

                    if (!roleResult.Succeeded)
                    {
                        throw new Exception("Problem at RoleCreating.....");
                    }
                }

                var superAdminUser = userManager.FindByEmailAsync(superAdminEmail).Result;

                if (superAdminUser == null)
                {
                    superAdminUser = new BookStoreUser
                    {
                        Email = superAdminEmail,
                        UserName = superAdminUserName,
                        EmailConfirmed = true,
                        Name = "Ramin",
                        Surname = "Safarli"
                    };

                    var userResult = userManager.CreateAsync(superAdminUser, superAdminPassword).Result;

                    if (!userResult.Succeeded)
                    {
                        throw new Exception("Problem at UserCreating.....");
                    }
                }

                var isInRole = userManager.IsInRoleAsync(superAdminUser, superAdminRole.Name).Result;


                if (isInRole != true)
                {
                    userManager.AddToRoleAsync(superAdminUser, superAdminRole.Name).Wait();
                }
            }

            return app;
        }
    }
}
