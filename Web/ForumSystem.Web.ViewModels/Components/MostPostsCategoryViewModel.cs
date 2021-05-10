namespace ForumSystem.Web.ViewModels.Components
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class MostPostsCategoryViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public string Image { get; set; }
    }
}
