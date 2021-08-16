namespace ForumSystem.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.PartialViews;

    public class CategoriesByUserListModel
    {
        public IEnumerable<CategoryByUserViewModel> Categories { get; set; }

        public PaginationViewModel PaginationModel { get; set; }

        public string SearchTerm { get; set; }
    }
}
