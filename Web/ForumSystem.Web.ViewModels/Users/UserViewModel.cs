namespace ForumSystem.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.PartialViews;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }

        public int PostsCount { get; set; }

        public PaginationViewModel PaginationModel { get; set; }

        public IEnumerable<UserPostsViewModel> Posts { get; set; }
    }
}
