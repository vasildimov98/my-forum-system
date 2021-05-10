namespace ForumSystem.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class HomePostsListViewModel
    {
        public IEnumerable<HomePostViewModel> Posts { get; set; }

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage - 1;

        public int NextPage => this.CurrentPage + 1;

        public int PagesCount { get; set; }

        public int TotalPosts { get; set; }
    }
}
