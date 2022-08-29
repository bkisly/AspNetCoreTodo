using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreTodo
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            await EnsureTestAdminAsync(userManager);
        }

        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(RoleName.Administrator))
                await roleManager.CreateAsync(new IdentityRole(RoleName.Administrator));
        }

        private static async Task EnsureTestAdminAsync(UserManager<IdentityUser> userManager)
        {
            var testAdmin = await userManager.Users.SingleOrDefaultAsync(user => user.UserName == "admin@todo.local");

            if(testAdmin == null)
            {
                testAdmin = new IdentityUser
                {
                    UserName = "admin@todo.local",
                    Email = "admin@todo.local"
                };

                await userManager.CreateAsync(testAdmin, "Te$t123");
                await userManager.AddToRoleAsync(testAdmin, RoleName.Administrator);
            }
        }
    }
}
