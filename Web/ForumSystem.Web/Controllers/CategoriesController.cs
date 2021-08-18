namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Categories;
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
        public async Task<IActionResult> All(int id, string searchTerm)
        {
            if (id <= 0)
            {
                id = 1;

                this.TempData[InvalidMessageKey] = InvalidPageRequest;

                return this.RedirectToAction(nameof(this.All), new { id, searchTerm });
            }

            var currentPage = id;

            var categoriesCount = this.categoriesService.GetCount(searchTerm);

            var totalPages = (int)Math.Ceiling((double)categoriesCount / CategoriesPerPage);

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

            var categories = await this.categoriesService
                .GetAllAsync<CategoryViewModel>(searchTerm, CategoriesPerPage, (currentPage - 1) * CategoriesPerPage);

            var viewModel = new CateogoriesListModel
            {
                Categories = categories,
                PaginationModel = new PaginationViewModel
                {
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    RouteName = "default",
                    SearchTerm = searchTerm,
                },
            };

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> ByOwner(
            string username,
            int id,
            string searchTerm)
        {
            var loggedInUser = await this.userManager
                .GetUserAsync(this.User);

            if (loggedInUser.UserName != username)
            {
                return this.Unauthorized();
            }

            if (id <= 0)
            {
                id = 1;

                this.TempData[InvalidMessageKey] = InvalidPageRequest;

                return this.RedirectToAction(nameof(this.ByOwner), new { username, id, searchTerm });
            }

            var currentPage = Math.Max(1, id);

            var categoriesCount = this.categoriesService
                .GetCountByOwner(username, searchTerm);

            var totalPages = (int)Math.Ceiling((double)categoriesCount / CategoriesPerPage);

            if (totalPages == 0)
            {
                totalPages = 1;
            }

            if (currentPage > totalPages)
            {
                currentPage = totalPages;

                this.TempData[InvalidMessageKey] = InvalidPageRequest;

                return this.RedirectToAction(nameof(this.ByOwner), new { username, id = currentPage, searchTerm });
            }

            var categories = await this.categoriesService
                .GetByOwnerUsernameAsync<CategoryByUserViewModel>(
                    username,
                    searchTerm,
                    CategoriesPerPage,
                    (currentPage - 1) * CategoriesPerPage);

            var viewModel = new CategoriesByUserListModel
            {
                Categories = categories,
                PaginationModel = new PaginationViewModel
                {
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    RouteName = "categories-username-page",
                    SearchTerm = searchTerm,
                },
            };

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> ByName(string name, int id, string searchTerm)
        {
            if (id <= 0)
            {
                id = 1;

                this.TempData[InvalidMessageKey] = InvalidPageRequest;

                return this.RedirectToAction(nameof(this.ByName), new { name, id, searchTerm });
            }

            var currentPage = id;

            name = name.Replace("-", " ");

            var category = await this.categoriesService
               .GetByNameAsync<CategoryPostsListModel>(name);

            if (category == null)
            {
                this.TempData[ErrorTitleKey] = ErrorNotFoundTitle;
                this.TempData[ErrorMessageKey] = ErrorNotFoundMessage;

                return this.RedirectToAction("Error", "Home");
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                category.PostsCount = this.postsService
                    .GetCountByCategoryName(name, searchTerm);
            }

            var totalPages = (int)Math.Ceiling((double)category.PostsCount / PostsPerPage);

            if (totalPages == 0)
            {
                totalPages = 1;
            }

            if (currentPage > totalPages)
            {
                currentPage = totalPages;

                this.TempData[InvalidMessageKey] = InvalidPageRequest;

                name = name.Replace(" ", "-");

                return this.RedirectToAction(nameof(this.ByName), new { name, id = currentPage, searchTerm });
            }

            category.Posts = await this.postsService
                .GetAllByCategoryIdAsync<PostListViewModel>(
                category.Id,
                searchTerm,
                PostsPerPage,
                (currentPage - 1) * PostsPerPage);

            category.PaginationModel = new PaginationViewModel
            {
                CurrentPage = currentPage,
                TotalPages = totalPages,
                RouteName = "category-name-page",
                SearchTerm = searchTerm,
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

            var user = await this.userManager
                .GetUserAsync(this.User);

            var isUserAdmin = this.User.IsInRole(AdministratorRoleName);

            var isCategoryNameTaken = await this.categoriesService
                .CreateAsync(input, user.Id, isUserAdmin);

            if (!isCategoryNameTaken)
            {
                this.TempData[InvalidMessageKey] = InvalidNameMessage;
                return this.View();
            }

            if (!isUserAdmin)
            {
                this.TempData[SuccessMessageKey] = SuccessCategoryCreate;
            }

            return this.RedirectToAction("ByOwner", "Categories", new { username = user.UserName, id = 1 });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await this.categoriesService
                .GetByIdAsync<CategoryEditModel>(id);

            if (category == null)
            {
                this.TempData[ErrorTitleKey] = ErrorNotFoundTitle;
                this.TempData[ErrorMessageKey] = ErrorNotFoundMessage;

                return this.RedirectToAction("Error", "Home");
            }

            if (!this.CheckIfLogInUserIsTheOwner(id))
            {
                return this.Unauthorized();
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
                var isUserAdmin = this.User.IsInRole(AdministratorRoleName);

                var user = await this.userManager
                        .GetUserAsync(this.User);

                await this.categoriesService
                    .EditAsync(isUserAdmin, user.Id, input);

                if (!isUserAdmin)
                {
                    this.TempData[SuccessMessageKey] = SuccessCategoryEdit;
                }

                return this.RedirectToAction("ByOwner", "Categories", new { username = user.UserName, id = 1 });
            }
            catch (InvalidOperationException)
            {
                return this.BadRequest();
            }
            catch (UnauthorizedAccessException)
            {
                return this.Unauthorized();
            }
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id, bool isFromAdminPanel = false)
        {
            var category = await this.categoriesService
                .GetByIdAsync<CategoryEditModel>(id);

            if (category == null)
            {
                this.TempData[ErrorTitleKey] = ErrorNotFoundTitle;
                this.TempData[ErrorMessageKey] = ErrorNotFoundMessage;

                return this.RedirectToAction("Error", "Home");
            }

            if (!this.CheckIfLogInUserIsTheOwner(id))
            {
                return this.Unauthorized();
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
                var isUserAdmin = this.User.IsInRole(AdministratorRoleName);

                var userId = this.userManager
                    .GetUserId(this.User);

                await this.categoriesService
                    .DeleteAsync(isUserAdmin, userId, id);
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
                return this.RedirectToAction("Index", "CategoriesAdmin", new { area = AdministratorAreaName, id = 1 });
            }

            var username = (await this.userManager.GetUserAsync(this.User)).UserName;

            return this.RedirectToAction("ByOwner", "Categories", new { area = string.Empty, username, id = 1 });
        }

        private bool CheckIfLogInUserIsTheOwner(int categoryId)
        {
            var isUserAdmin = this.User.IsInRole(AdministratorRoleName);

            if (isUserAdmin)
            {
                return true;
            }

            var userId = this.userManager
           .GetUserId(this.User);

            var isUserTheOwner = this.categoriesService
                .IsUserTheOwner(categoryId, userId);

            if (!isUserTheOwner)
            {
                return false;
            }

            return true;
        }
    }
}
