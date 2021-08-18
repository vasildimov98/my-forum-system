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

    using static ForumSystem.Common.GlobalConstants;

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

        public async Task<IActionResult> All(int? id, string searchTerm)
        {
            if (!id.HasValue)
            {
                id = 1;
            }

            if (id <= 0)
            {
                id = 1;

                this.TempData[InvalidMessageKey] = InvalidPageRequest;

                return this.RedirectToAction(nameof(this.All), new { id, searchTerm });
            }

            var currentPage = id.Value;

            var postsCount = this.postsService.GetCount(searchTerm);

            var totalPages = (int)Math.Ceiling((double)postsCount / PostsPerPage);

            if (totalPages == 0)
            {
                totalPages = 1;
            }

            if (currentPage > totalPages)
            {
                currentPage = totalPages;

                this.TempData[InvalidMessageKey] = InvalidPageRequest;

                return this.RedirectToAction(nameof(this.All), new { id = currentPage, searchTerm });
            }

            var posts = await this.postsService
                    .GetAllAsync<PostListViewModel>(
                    searchTerm,
                    PostsPerPage,
                    (currentPage - 1) * PostsPerPage);

            var postsList = new PostsAllViewModel
            {
                PostsCount = postsCount,
                Posts = posts,
                PaginationModel = new PaginationViewModel
                {
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    RouteName = "default",
                    SearchTerm = searchTerm,
                },
            };

            return this.View(postsList);
        }

        [Authorize]
        public async Task<IActionResult> ById(int id, string title)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var post = this.postsService
                .GetByIdAndTitle<PostViewModel>(id, title.Replace("_", " "));

            if (post == null)
            {
                this.TempData[ErrorTitleKey] = ErrorNotFoundTitle;
                this.TempData[ErrorMessageKey] = ErrorNotFoundMessage;

                return this.RedirectToAction("Error", "Home");
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
            if (input.Title.Contains(Environment.NewLine))
            {
                this.ModelState.AddModelError(string.Empty, "Title is not allowed to have new line!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var userId = this.userManager
                .GetUserId(this.User);

            var id = await this.postsService.CreateAsync(input.Title, input.Content, input.CategoryId, userId);

            var title = input.Title.Replace(" ", "_");

            return this.RedirectToAction(nameof(this.ById), new { id, title });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var post = this.postsService
                .GetById<PostEditModel>(id);

            if (post == null)
            {
                this.TempData[ErrorTitleKey] = ErrorNotFoundTitle;
                this.TempData[ErrorMessageKey] = ErrorNotFoundMessage;

                return this.RedirectToAction("Error", "Home");
            }

            if (!this.CheckIfLogInUserIsTheOwner(id))
            {
                return this.Unauthorized();
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
                var isUserAdmin = this.User.IsInRole(AdministratorRoleName);

                var userId = this.userManager
                    .GetUserId(this.User);

                await this.postsService
                    .EditAsync(
                        isUserAdmin,
                        userId,
                        editModel.Id,
                        editModel.Title,
                        editModel.Content,
                        editModel.CategoryId);
            }
            catch (InvalidOperationException)
            {
                return this.BadRequest();
            }
            catch (UnauthorizedAccessException)
            {
                return this.Unauthorized();
            }

            return this.RedirectToAction("ById", "Posts", new { editModel.Id, editModel.Title, area = string.Empty });
        }

        [Authorize]
        public IActionResult Delete(int id, bool isFromAdminPanel)
        {
            var post = this.postsService
                .GetById<PostEditModel>(id);

            if (post == null)
            {
                this.TempData[ErrorTitleKey] = ErrorNotFoundTitle;
                this.TempData[ErrorMessageKey] = ErrorNotFoundMessage;

                return this.RedirectToAction("Error", "Home");
            }

            if (!this.CheckIfLogInUserIsTheOwner(id))
            {
                return this.Unauthorized();
            }

            post.IsFromAdminPanel = isFromAdminPanel;

            return this.View(post);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id, bool isFromAdminPanel)
        {
            try
            {
                var isUserAdmin = this.User.IsInRole(AdministratorRoleName);

                var userId = this.userManager
                    .GetUserId(this.User);

                await this.postsService
                    .DeleteAsync(
                        isUserAdmin,
                        userId,
                        id);
            }
            catch (InvalidOperationException)
            {
                return this.BadRequest();
            }
            catch (UnauthorizedAccessException)
            {
                return this.Unauthorized();
            }

            if (isFromAdminPanel)
            {
                return this.RedirectToAction("Index", "PostsAdmin", new { area = AdministratorAreaName, id = 1 });
            }

            return this.RedirectToAction("All", "Posts", new { area = string.Empty, id = 1 });
        }

        private bool CheckIfLogInUserIsTheOwner(int postId)
        {
            var isUserAdmin = this.User.IsInRole(AdministratorRoleName);

            if (!isUserAdmin)
            {
                var userId = this.userManager
               .GetUserId(this.User);

                var isUserTheOwner = this.postsService
                    .IsUserTheOwner(postId, userId);

                if (!isUserTheOwner)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
