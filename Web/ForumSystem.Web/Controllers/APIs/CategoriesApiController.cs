namespace ForumSystem.Web.Controllers.APIs
{
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/categories")]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ICategoriesService categories;

        public CategoriesApiController(ICategoriesService categories)
        {
            this.categories = categories;
        }

        [HttpGet]
        [Authorize]
        [Route("search")]
        public async Task<IActionResult> Search(string term)
        {
            if (!string.IsNullOrWhiteSpace(term))
            {
                var data = await this.categories
                    .FindCategoryByTermSearchAsync<CategoryDropDownViewModel>(term);

                return this.Ok(data);
            }
            else
            {
                var data = await this.categories
                   .GetAllAsync<CategoryDropDownViewModel>();

                return this.Ok(data);
            }
        }
    }
}
