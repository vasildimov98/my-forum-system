namespace ForumSystem.Web.ViewModels.Administration.Categories
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.PartialViews;

    public class CategoryCrudModelList
    {
        public IEnumerable<CategoryCrudModel> Categories { get; set; }

        public PaginationViewModel PaginationModel { get; set; }

        public string SearchTerm { get; set; }
    }
}
