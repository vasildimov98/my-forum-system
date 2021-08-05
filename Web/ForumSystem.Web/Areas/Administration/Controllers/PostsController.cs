namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Administration.Posts;
    using ForumSystem.Web.ViewModels.PartialViews;
    using ForumSystem.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : AdministrationController
    {
        private const int PostsPerPage = 5;

        private readonly IPostsService postsService;
        private readonly ICategoriesService categoriesService;

        public PostsController(
            IPostsService postsService,
            ICategoriesService categoriesService)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var page = Math.Max(1, id);

            this.TempData["page"] = page;

            var posts = await this.postsService
                .GetAllAsync<PostCrudModel>(PostsPerPage, (page - 1) * PostsPerPage);

            var postCount = this.postsService.GetCount();

            var pagesCount = (int)Math.Ceiling((decimal)postCount / PostsPerPage);

            var viewModel = new PostCrudModelList
            {
                Posts = posts,
                PaginationModel = new PaginationViewModel
                {
                    CurrentPage = page,
                    PagesCount = pagesCount,
                    RouteName = "areaRoute",
                },
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await this.postsService
                .GetByIdAsync<PostEditModel>(id);

            var categories = await this.categoriesService
                .GetAllAsync<CategoryDropDownViewModel>();

            post.Categories = categories;

            if (post == null)
            {
                return this.NotFound();
            }

            post.CurrentPage = Convert
                .ToInt32(this.TempData["page"]);

            return this.View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostEditModel editModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Edit), new { editModel.Id });
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var post = await this.postsService
                .GetByIdAsync<PostEditModel>(id.Value);

            if (post == null)
            {
                return this.NotFound();
            }

            post.CurrentPage = Convert
                .ToInt32(this.TempData["page"]);

            return this.View(post);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
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

            return this.RedirectToAction("Posts", "Home", new { area = string.Empty });
        }
    }
}
