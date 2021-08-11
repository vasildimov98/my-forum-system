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
                    TotalPages = pagesCount,
                    RouteName = "default",
                },
            };

            return this.View(postsList);
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
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var userId = this.userManager
                .GetUserId(this.User);

            var id = await this.postsService.CreateAsync(input.Title, input.Content, input.CategoryId, userId);

            return this.RedirectToAction(nameof(this.ById), new { id });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var post = this.postsService
                .GetById<PostEditModel>(id);

            if (post == null)
            {
                return this.NotFound();
            }

            var categories = await this.categoriesService
                .GetAllAsync<CategoryDropDownViewModel>();

            post.Categories = categories;


            return this.View(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(PostEditModel editModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(editModel.Id);
            }

            try
            {
                await this.postsService
                .EditAsync(
                    editModel.Id,
                    editModel.Title,
                    editModel.Content,
                    editModel.CategoryId);
            }
            catch
            {
                return this.NotFound();
            }

            return this.RedirectToAction("ById", "Posts", new { editModel.Id, area = string.Empty });
        }

        [Authorize]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var post = this.postsService
                .GetById<PostEditModel>(id.Value);

            if (post == null)
            {
                return this.NotFound();
            }

            return this.View(post);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await this.postsService.DeleteAsync(id);
            }
            catch
            {
                return this.NotFound();
            }

            return this.RedirectToAction("All", "Posts", new { area = string.Empty, id = 1 });
        }
    }
}
