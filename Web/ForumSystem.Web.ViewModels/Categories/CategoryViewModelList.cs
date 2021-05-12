namespace ForumSystem.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.PartialViews;

    public class CategoryViewModelList
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public PaginationViewModel PaginationModel { get; set; }
    }
}
