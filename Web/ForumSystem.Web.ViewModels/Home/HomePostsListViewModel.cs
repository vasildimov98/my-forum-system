namespace ForumSystem.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.PartialViews;

    public class HomePostsListViewModel
    {
        public IEnumerable<HomePostViewModel> Posts { get; set; }

        public PaginationViewModel PaginationModel { get; set; }
    }
}
