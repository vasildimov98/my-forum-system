namespace ForumSystem.Web.Tests
{
    using ForumSystem.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ForumSystem.Web.Tests.Mock;

    using MyTested.AspNetCore.Mvc;
    using ForumSystem.Services.Data;

    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
        }

        public void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.ReplaceTransient<UserManager<ApplicationUser>>(_ => UserManagerMock.Create);
        }
    }
}
