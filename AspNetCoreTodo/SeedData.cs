using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNetCoreTodo.Data;

namespace AspNetCoreTodo
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, string testUserPw)
        {
            using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            var adminId = await EnsureUser(serviceProvider, testUserPw, "admin@todo.local");
            await EnsureRole(serviceProvider, adminId, RoleName.Administrator);
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string testUserPw, string testUserName)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var user = await userManager.FindByNameAsync(testUserName);

            if(user == null)
            {
                user = new IdentityUser
                {
                    UserName = testUserName,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null) throw new Exception("The password is not strong enough.");

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string testUserId, string roleName)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (roleManager == null) throw new NullReferenceException("RoleManager is null.");

            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var user = await userManager.FindByIdAsync(testUserId);

            if (userManager == null) throw new NullReferenceException("UserManager is null.");
            if (user == null) throw new Exception("The password is not strong enough.");

            return await userManager.AddToRoleAsync(user, roleName);
        }
    }
}
