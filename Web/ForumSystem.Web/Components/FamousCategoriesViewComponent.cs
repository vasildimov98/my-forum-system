namespace ForumSystem.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Components;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using static ForumSystem.Common.GlobalConstants;

    public class FamousCategoriesViewComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;
        private readonly IMemoryCache cache;

        public FamousCategoriesViewComponent(
            ICategoriesService categoriesService,
            IMemoryCache cache)
        {
            this.categoriesService = categoriesService;
            this.cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var famousCategories = this.cache
                .Get<IEnumerable<FamousCategoryViewModel>>(FamousCategoriesKey);

            if (famousCategories == null)
            {
                famousCategories = await this.categoriesService
                .GetMostFamousCategories<FamousCategoryViewModel>();

                var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                this.cache
                    .Set(FamousCategoriesKey, famousCategories, options);
            }

            var viewModel = new FamousCategoriesListModel
            {
                Categories = famousCategories,
            };

            return this.View(viewModel);
        }
    }
}
