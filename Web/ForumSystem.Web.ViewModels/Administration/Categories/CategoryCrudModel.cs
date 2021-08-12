namespace ForumSystem.Web.ViewModels.Administration.Categories
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class CategoryCrudModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public bool IsApprovedByAdmin { get; set; }
    }
}
