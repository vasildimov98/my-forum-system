namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Posts;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IPostService postsService;
        private readonly UserManager<ApplicationUser> userManager;

        public PostsController(
            ICategoriesService categoriesService,
            IPostService postsService,
            UserManager<ApplicationUser> userManager)
        {
            this.categoriesService = categoriesService;
            this.postsService = postsService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var categories = this.categoriesService.GetAllAsync<CategoryDropDownViewModel>();

            var inputModel = new PostInputModel
            {
                Categories = await categories,
            };

            return this.View(inputModel);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostInputModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var id = await this.postsService.CreateAsync(input.Title, input.Content, input.CategoryId, user.Id);

            return this.RedirectToAction(nameof(this.ById), new { id });
        }

        [Authorize]
        public async Task<IActionResult> ById(int id)
        {
            var post = await this.postsService.GetByIdAsync<PostViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            return this.View(post);
        }
    }
}
