namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;

    using Microsoft.AspNetCore.Mvc;

    public class PostsController : BaseController
    {
        private readonly ISettingsService settingsService;

        private readonly IDeletableEntityRepository<Setting> repository;

        public PostsController(ISettingsService settingsService, IDeletableEntityRepository<Setting> repository)
        {
            this.settingsService = settingsService;
            this.repository = repository;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        public async Task<IActionResult> InsertSetting()
        {
            var random = new Random();
            var setting = new Setting { Name = $"Name_{random.Next()}", Value = $"Value_{random.Next()}" };

            await this.repository.AddAsync(setting);
            await this.repository.SaveChangesAsync();

            return this.RedirectToAction(nameof(this.Create));
        }
    }
}
