namespace ForumSystem.Web.ViewModels.Users
{
    using Microsoft.AspNetCore.Http;

    public class EditImageInputModel
    {
        public IFormFile Image { get; set; }
    }
}
