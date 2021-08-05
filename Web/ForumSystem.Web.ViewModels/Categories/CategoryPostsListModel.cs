namespace ForumSystem.Web.ViewModels.Chat
{
    using System.Collections.Generic;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.PartialViews;
    using ForumSystem.Web.ViewModels.Posts;
    using Ganss.XSS;

    public class CategoryPostsListModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ChatRoute => $"/Chat/{this.Name.Replace(" ", "-")}";

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string SanitizeDescription => new HtmlSanitizer().Sanitize(this.Description);

        public IEnumerable<PostListViewModel> Posts { get; set; }

        public int PostsCount { get; set; }

        public PaginationViewModel PaginationModel { get; set; }
    }
}
