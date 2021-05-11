namespace ForumSystem.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Categories;

    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> ByName(string name)
        {
            var category = await this.categoriesService.GetByNameAsync<CategoryPostsListModel>(name);

            category.Posts = category.Posts
                .OrderByDescending(x => x.CreatedOn);

            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }
    }
}
