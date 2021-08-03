namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Posts;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class PostsController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;
        private readonly ICommentsService commentsService;
        private readonly UserManager<ApplicationUser> userManager;

        public PostsController(
            ICategoriesService categoriesService,
            IPostsService postsService,
            ICommentsService commentsService,
            UserManager<ApplicationUser> userManager)
        {
            this.categoriesService = categoriesService;
            this.postsService = postsService;
            this.commentsService = commentsService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Create(string selected = null)
        {
            var categories = await this.categoriesService
                .GetAllAsync<CategoryDropDownViewModel>();

            var inputModel = new PostInputModel
            {
                Categories = categories,
                Selected = selected,
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
            var user = await this.userManager.GetUserAsync(this.User);
            var post = await this.postsService
                .GetByIdAsync<PostViewModel>(id);

            if (post == null)
            {
                return this.NotFound();
            }

            foreach (var comment in post.Comments)
            {
                comment.IsSignInUserTheOwnerOfComment = this.commentsService
                    .IsSignInUserTheOwenerOfComment(comment.Id, user.Id);
            }

            post.Comments = post.Comments;

            post.LoggedInUserName = user.UserName;

            return this.View(post);
        }
    }
}
