namespace ForumSystem.Web.ViewModels.Components
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class FamousCategoryViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Url => $"/Category/{this.Name.Replace(" ", "-")}/1";
    }
}
