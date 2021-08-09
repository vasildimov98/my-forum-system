namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.PartialViews;
    using ForumSystem.Web.ViewModels.Posts;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : BaseController
    {
        private const int PostsPerPage = 5;

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

        public async Task<IActionResult> All(int id = 1)
        {
            var page = id;

            var posts = await this.postsService
                    .GetAllAsync<PostListViewModel>(PostsPerPage, (page - 1) * PostsPerPage);

            var count = this.postsService.GetCount();
            var pagesCount = (int)Math.Ceiling((double)count / PostsPerPage);
            var postsList = new PostsAllViewModel
            {
                PostsCount = count,
                Posts = posts,
                PaginationModel = new PaginationViewModel
                {
                    CurrentPage = page,
                    PagesCount = pagesCount,
                    RouteName = "default",
                },
            };

            return this.View(postsList);
        }

        [Authorize]
        public async Task<IActionResult> Create(string selected)
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
            var userId = this.userManager
                .GetUserId(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var id = await this.postsService.CreateAsync(input.Title, input.Content, input.CategoryId, userId);

            return this.RedirectToAction(nameof(this.ById), new { id });
        }

        [Authorize]
        public async Task<IActionResult> ById(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var post = this.postsService
                .GetById<PostViewModel>(id);

            if (post == null)
            {
                return this.NotFound();
            }

            post.IsSignInUserTheOwnerOfThePost = user.UserName == post.UserUserName;

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
