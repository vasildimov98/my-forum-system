namespace ForumSystem.Web.ViewModels.Categories
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class CategoryLiveChatViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }
    }
}
