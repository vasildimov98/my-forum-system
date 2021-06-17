namespace ForumSystem.Web.Hub
{
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Chat;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IChatsService chatsSercvice;

        public ChatHub(
            UserManager<ApplicationUser> userManager,
            IChatsService chatsSercvice)
        {
            this.userManager = userManager;
            this.chatsSercvice = chatsSercvice;
        }

        public async Task Send(string message, string categoryName)
        {
            var userId = this.userManager
                .GetUserId(this.Context.User);

            var createdOn = await this.chatsSercvice
                .CreateMessageAsync(categoryName, userId, message);

            await this.Clients.All
                .SendAsync(
                "NewMessage",
                new MessageViewModel
                {
                    User = this.Context.User.Identity.Name,
                    Content = message,
                    CreatedOn = createdOn,
                });
        }
    }
}
