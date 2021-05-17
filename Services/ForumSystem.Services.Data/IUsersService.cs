namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IUsersService
    {
        Task<string> ChangeUsername(string username, ApplicationUser user);

        Task<string> UploadProfileImage(IFormFile image, string userId, string path);
    }
}
