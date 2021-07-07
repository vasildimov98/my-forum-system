namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Administration.Posts;
    using ForumSystem.Web.ViewModels.PartialViews;
    using ForumSystem.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : AdministrationController
    {
        private const int PostsPerPage = 5;

        private readonly IPostsService postsService;

        public PostsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var page = Math.Max(1, id);

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
            var post = await this.postsService.GetByIdAsync<PostEditModel>(id);

            if (post == null)
            {
                return this.NotFound();
            }

            return this.View(post);
        }
    }
}
