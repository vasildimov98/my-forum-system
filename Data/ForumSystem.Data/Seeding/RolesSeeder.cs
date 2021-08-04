namespace ForumSystem.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    using ForumSystem.Common;
    using ForumSystem.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal class RolesSeeder : ISeeder
    {
        private IConfiguration configuration;

        public RolesSeeder(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRoleAsync(
                roleManager,
                userManager,
                GlobalConstants.AdministratorRoleName,
                this.configuration.GetSection("AdminCredentials:Username").Value,
                this.configuration.GetSection("AdminCredentials:Email").Value,
                this.configuration.GetSection("AdminCredentials:Password").Value);
        }

        private static async Task SeedRoleAsync(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            string roleName,
            string username,
            string email,
            string password)
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
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                };

                await userManager.CreateAsync(user, password);
            }

            var isUserAdmin = await userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);

            // check if user is admin already
            if (!isUserAdmin)
            {
                await userManager
                    .AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
