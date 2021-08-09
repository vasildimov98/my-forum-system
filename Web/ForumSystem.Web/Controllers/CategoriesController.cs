namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Chat;
    using ForumSystem.Web.ViewModels.PartialViews;
    using ForumSystem.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using static ForumSystem.Common.GlobalConstants;

    public class CategoriesController : BaseController
    {
        private const int PostsPerPage = 5;
        private const int CategoriesPerPage = 10;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;

        public CategoriesController(
            UserManager<ApplicationUser> userManager,
            ICategoriesService categoriesService,
            IPostsService postsService)
        {
            this.userManager = userManager;
            this.categoriesService = categoriesService;
            this.postsService = postsService;
        }

        [Authorize]
        public async Task<IActionResult> All(int id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var page = Math.Max(1, id);

            var categories = await this.categoriesService
                .GetAllAsync<CategoryViewModel>(CategoriesPerPage, (page - 1) * CategoriesPerPage);

            if (!this.User.IsInRole(AdministratorRoleName))
            {
                foreach (var category in categories)
                {
                    category.IsSignInUserTheOwner = category.OwnerId == userId ? true : false;
                }
            }

            var categoriesCount = this.categoriesService.GetCount();

            var pagesCount = (int)Math.Ceiling((double)categoriesCount / CategoriesPerPage);

            var viewModel = new CategoryViewModelList
            {
                Categories = categories,
                PaginationModel = new PaginationViewModel
                {
                    CurrentPage = page,
                    PagesCount = pagesCount,
                    RouteName = "default",
                },
            };

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> ByName(string name, int id)
        {
            var page = id;

            name = name.Replace("-", " ");

            var category = await this.categoriesService
                .GetByNameAsync<CategoryPostsListModel>(name);

            if (category == null)
            {
                return this.NotFound();
            }

            category.Posts = await this.postsService
                .GetAllByCategoryIdAsync<PostListViewModel>(category.Id, PostsPerPage, (page - 1) * PostsPerPage);

            var pagesCount = (int)Math.Ceiling((double)category.PostsCount / PostsPerPage);

            category.PaginationModel = new PaginationViewModel
            {
                CurrentPage = page,
                PagesCount = pagesCount,
                RouteName = "category-name-page",
            };

            return this.View(category);
        }
    }
}
