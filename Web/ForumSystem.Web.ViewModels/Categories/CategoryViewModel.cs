namespace ForumSystem.Web.ViewModels.Chat
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string RouteName => this.Name.Replace(" ", "-");

        public string ImageUrl { get; set; }

        public string UserId { get; set; }

        public bool IsSignInUserTheOwner { get; set; }

        public string OwnerId { get; set; }
    }
}
