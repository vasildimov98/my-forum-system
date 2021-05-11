namespace ForumSystem.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class CategoryPostsListModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public IEnumerable<CategoryPostViewModel> Posts { get; set; }

        public int PostsCount { get; set; }
    }
}
