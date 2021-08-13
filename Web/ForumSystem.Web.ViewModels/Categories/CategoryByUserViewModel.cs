namespace ForumSystem.Web.ViewModels.Categories
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class CategoryByUserViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string RouteName => this.Name.Replace(" ", "-");

        public string ImageUrl { get; set; }
    }
}
