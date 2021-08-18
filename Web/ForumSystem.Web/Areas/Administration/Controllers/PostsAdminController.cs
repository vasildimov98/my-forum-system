namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Administration.Posts;
    using ForumSystem.Web.ViewModels.PartialViews;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static ForumSystem.Common.GlobalConstants;

    public class PostsAdminController : AdministrationController
    {
        private const int PostsPerPage = 5;

        private readonly IPostsService postsService;

        public PostsAdminController(
            IPostsService postsService)
        {
            this.postsService = postsService;
        }

        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Index(int id, string searchTerm)
        {
            if (id <= 0)
            {
                id = 1;

                this.TempData[InvalidMessageKey] = InvalidPageRequest;

                return this.RedirectToAction(nameof(this.Index), new { area = AdministratorAreaName, id, searchTerm });
            }

            var currentPage = id;

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

                return this.RedirectToAction(nameof(this.Index), new { area = AdministratorAreaName, id = currentPage, searchTerm });
            }

            var posts = await this.postsService
                .GetAllAsync<PostCrudModel>(
                searchTerm,
                PostsPerPage,
                (currentPage - 1) * PostsPerPage);

            var viewModel = new PostCrudModelList
            {
                Posts = posts,
                PaginationModel = new PaginationViewModel
                {
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    RouteName = "areaRoute",
                    SearchTerm = searchTerm,
                },
            };

            return this.View(viewModel);
        }
    }
}
