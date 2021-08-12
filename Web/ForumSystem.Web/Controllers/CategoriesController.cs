namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Categories;
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
                    TotalPages = pagesCount,
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
                TotalPages = pagesCount,
                RouteName = "category-name-page",
            };

            return this.View(category);
        }

        [Authorize]
        public IActionResult Create()
            => this.View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CategoryInputModel input)
        {
            if (input.Name.Contains(Environment.NewLine))
            {
                this.ModelState.AddModelError(string.Empty, "Name of community cannot contain new line!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var userId = this.userManager
                .GetUserId(this.User);

            var isUserAdmin = this.User.IsInRole(AdministratorRoleName);

            var isCategoryNameTaken = await this.categoriesService
                .CreateAsync(input, userId, isUserAdmin);

            if (!isCategoryNameTaken)
            {
                this.TempData[InvalidMessageKey] = InvalidNameMessage;
                return this.View();
            }

            if (!isUserAdmin)
            {
                this.TempData[SuccessMessageKey] = SuccessCategoryCreate;
            }

            return this.RedirectToAction("All", "Categories", new { id = 1 });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await this.categoriesService
                .GetByIdAsync<CategoryEditModel>(id);

            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryEditModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input.Id);
            }

            try
            {
                await this.categoriesService
                    .EditAsync(input.Id, input);
            }
            catch
            {
                return this.NotFound();
            }

            return this.RedirectToAction("ByName", "Categories", new { name = input.Name, id = 1 });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id, bool isFromAdminPanel = false)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = await this.categoriesService
                .GetByIdAsync<CategoryEditModel>((int)id);

            if (category == null)
            {
                return this.NotFound();
            }

            category.IsFromAdminPanel = isFromAdminPanel;

            return this.View(category);
        }

        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, bool isFromAdminPanel)
        {
            try
            {
                await this.categoriesService
                    .DeleteAsync(id);
            }
            catch
            {
                return this.NotFound();
            }

            if (isFromAdminPanel)
            {
                return this.RedirectToAction("Index", "CategoriesAdmin", new { area = AdministratorAreaName });
            }

            return this.RedirectToAction("All", "Categories", new { area = string.Empty, id = 1 });
        }
    }
}
