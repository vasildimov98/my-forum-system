namespace ForumSystem.Web.ViewModels.Chat
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public string RouteName => this.Name.Replace(" ", "-");

        public string ImageUrl { get; set; }
    }
}
