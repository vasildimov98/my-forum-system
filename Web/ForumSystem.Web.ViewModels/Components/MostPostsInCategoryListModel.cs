namespace ForumSystem.Web.ViewModels.Components
{
    using System.Collections.Generic;

    public class MostPostsInCategoryListModel
    {
        public IEnumerable<MostPostsInCategoryViewModel> Categories { get; set; }
    }
}
