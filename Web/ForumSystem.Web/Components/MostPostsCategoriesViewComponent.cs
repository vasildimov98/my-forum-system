namespace ForumSystem.Web.Components
{
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Components;

    using Microsoft.AspNetCore.Mvc;

    public class MostPostsCategoriesViewComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;

        public MostPostsCategoriesViewComponent(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await this.categoriesService
                .GetMostPostsCategories<MostPostsCategoryViewModel>();

            var viewModel = new MostPostsCategoriesListModel
            {
                Categories = categories,
            };

            return this.View(viewModel);
        }
    }
}
