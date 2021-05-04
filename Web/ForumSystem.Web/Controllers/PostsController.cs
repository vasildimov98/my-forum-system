namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Posts;

    using Microsoft.AspNetCore.Mvc;

    public class PostsController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public PostsController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Create()
        {
            var categories = this.categoriesService.GetAll<CategoryDropDownViewModel>();

            var inputModel = new PostInputModel
            {
                Categories = categories,
            };

            return this.View(inputModel);
        }
    }
}
