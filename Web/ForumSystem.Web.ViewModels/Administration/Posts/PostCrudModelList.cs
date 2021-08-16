namespace ForumSystem.Web.ViewModels.Administration.Posts
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.PartialViews;

    public class PostCrudModelList
    {
        public IEnumerable<PostCrudModel> Posts { get; set; }

        public PaginationViewModel PaginationModel { get; set; }

        public string SearchTerm { get; set; }
    }
}
