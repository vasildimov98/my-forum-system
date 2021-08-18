namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Administration.Categories;
    using ForumSystem.Web.ViewModels.PartialViews;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static ForumSystem.Common.GlobalConstants;

    public class CategoriesAdminController : AdministrationController
    {
        private const int CategoriesPerPage = 5;

        private readonly ICategoriesService categoriesService;

        public CategoriesAdminController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
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

            var categoriesCount = this.categoriesService.GetCount(searchTerm, false);

            var totalPages = (int)Math.Ceiling((double)categoriesCount / CategoriesPerPage);

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

            var categories = await this.categoriesService
                .GetAllAsync<CategoryCrudModel>(
                         searchTerm,
                         CategoriesPerPage,
                         (currentPage - 1) * CategoriesPerPage,
                         false);

            var viewModel = new CategoryCrudModelList
            {
                Categories = categories,
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

        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Approve(int id, int page)
        {
            var isApproved = await this.categoriesService
                .ApproveCategoryAsync(id);

            if (!isApproved)
            {
                this.TempData[InvalidMessageKey] = InvalidApprovalMessage;
                return this.RedirectToAction(nameof(this.Index), new { area = AdministratorAreaName, id = page });
            }

            return this.RedirectToAction(nameof(this.Index), new { area = AdministratorAreaName, id = page });
        }
    }
}
