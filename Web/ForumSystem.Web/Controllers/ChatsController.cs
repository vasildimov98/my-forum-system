namespace ForumSystem.Web.Controllers
{
    using ForumSystem.Web.ViewModels.Chat;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ChatsController : Controller
    {
        [Authorize]
        public IActionResult LiveChat(string name)
        {
            var viewModel = new LiveChatViewModel
            {
                Name = name.Replace("-", " "),
            };

            return this.View(viewModel);
        }
    }
}
