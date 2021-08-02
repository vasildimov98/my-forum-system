namespace ForumSystem.Web.Controllers
{
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Chat;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ChatsController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IChatsService chatsService;

        public ChatsController(
            ICategoriesService categoriesService,
            IChatsService chatsService)
        {
            this.categoriesService = categoriesService;
            this.chatsService = chatsService;
        }

        [Authorize]
        public async Task<IActionResult> LiveChat(string name)
        {
            name = name.Replace("-", " ");
            var categoryId = this.categoriesService.GetIdCategoryIdByName(name);

            if (categoryId == 0)
            {
                return this.NotFound();
            }

            var viewModel = new LiveChatViewModel
            {
                Name = name,
                Messages = await this.chatsService
                    .GetAllMessagesByCategoryId<MessageViewModel>(categoryId),
            };

            return this.View(viewModel);
        }
    }
}
