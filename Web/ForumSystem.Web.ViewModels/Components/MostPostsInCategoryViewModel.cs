namespace ForumSystem.Web.ViewModels.Components
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class MostPostsInCategoryViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public string Url => $"/Category/{this.Name.Replace(" ", "-")}/1";
    }
}
