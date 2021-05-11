namespace ForumSystem.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.PartialViews;

    public class CategoryPostsListModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public IEnumerable<CategoryPostViewModel> Posts { get; set; }

        public int PostsCount { get; set; }

        public PaginationViewModel PaginationModel { get; set; }
    }
}
