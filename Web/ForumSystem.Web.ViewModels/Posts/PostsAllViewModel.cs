namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.PartialViews;

    public class PostsAllViewModel
    {
        public int PostsCount { get; set; }

        public IEnumerable<PostListViewModel> Posts { get; set; }

        public PaginationViewModel PaginationModel { get; set; }
    }
}
