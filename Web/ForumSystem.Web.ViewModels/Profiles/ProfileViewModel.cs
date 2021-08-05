namespace ForumSystem.Web.ViewModels.Profiles
{
    using System;
    using System.Collections.Generic;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.PartialViews;
    using ForumSystem.Web.ViewModels.Posts;

    public class ProfileViewModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }

        public int PostsCount { get; set; }

        public string ImageSrc { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsLoggedInUser { get; set; }

        public PaginationViewModel PaginationModel { get; set; }

        public IEnumerable<PostListViewModel> Posts { get; set; }
    }
}
