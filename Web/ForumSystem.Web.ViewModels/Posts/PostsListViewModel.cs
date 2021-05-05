namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    public class PostsListViewModel
    {
        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}
