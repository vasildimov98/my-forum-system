namespace ForumSystem.Web.Hub
{
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.ViewModels.Chat;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ChatHub(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task Send(string message)
        {
            await this.Clients.All
                .SendAsync(
                "NewMessage",
                new MessageViewModel
                {
                    User = this.Context.User.Identity.Name,
                    Content = message,
                });
        }
    }
}
