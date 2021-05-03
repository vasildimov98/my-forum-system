namespace ForumSystem.Web.ViewModels.Administration.Categories
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class CategoryInputModel
    {
        public string Name { get; set; }

        public string Descriptions { get; set; }

        public string Image { get; set; }
    }
}
