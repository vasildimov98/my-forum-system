namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IUsersService
    {
        Task<string> UploadProfileImage(IFormFile image, string userId, string path);
    }
}
