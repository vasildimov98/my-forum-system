namespace ForumSystem.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.PartialViews;

    public class CategoriesByUserViewModelList
    {
        public IEnumerable<CategoriesByUserViewModel> Categories { get; set; }

        public PaginationViewModel PaginationModel { get; set; }
    }
}
