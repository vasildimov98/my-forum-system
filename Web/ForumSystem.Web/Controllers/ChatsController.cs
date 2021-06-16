namespace ForumSystem.Web.Controllers
{
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Chat;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ChatsController : Controller
    {
        private readonly ICategoriesService categoriesService;

        public ChatsController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        [Authorize]
        public IActionResult LiveChat(string name)
        {
            name = name.Replace("-", " ");

            if (!this.categoriesService.ValidateCategoryName(name))
            {
                return this.NotFound();
            }

            var viewModel = new LiveChatViewModel
            {
                Name = name,
            };

            return this.View(viewModel);
        }
    }
}
