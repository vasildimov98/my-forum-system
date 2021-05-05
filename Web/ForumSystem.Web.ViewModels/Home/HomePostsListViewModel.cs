namespace ForumSystem.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class HomePostsListViewModel
    {
        public IEnumerable<HomePostViewModel> Posts { get; set; }
    }
}
