namespace ForumSystem.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Common;
    using ForumSystem.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRoleAsync(roleManager, userManager, GlobalConstants.AdministratorRoleName);
        }

        private static async Task SeedRoleAsync(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

            // find the user with the admin email
            var user = await userManager.FindByEmailAsync("vasil.dimov91@gmail.com");
            var isUserAdmin = await userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);

            // check if the user exists
            if (user != null
                && !isUserAdmin)
            {
                await userManager
                    .AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
