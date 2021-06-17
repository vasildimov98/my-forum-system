namespace ForumSystem.Web.Hub
{
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Chat;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

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
            var user = await this.userManager.Users
                .Include(x => x.ProfileImage)
                .FirstOrDefaultAsync(x => x.UserName == this.Context.User.Identity.Name);

            var createdOn = await this.chatsSercvice
                .CreateMessageAsync(categoryName, user.Id, message);

            await this.Clients.All
                .SendAsync(
                "NewMessage",
                new MessageViewModel
                {
                    User = this.Context.User.Identity.Name,
                    Content = message,
                    CreatedOn = createdOn,
                    ImageSrc = user.HasImage ?
                        "/profileImages/" + user.ProfileImage.Id + user.ProfileImage.Extention :
                        "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png",
                });
        }
    }
}
