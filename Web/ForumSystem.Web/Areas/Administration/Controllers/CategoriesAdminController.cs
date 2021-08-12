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
        private const int CategoryPerPage = 5;

        private readonly ICategoriesService categorieService;

        public CategoriesAdminController(ICategoriesService categoriesService)
        {
            this.categorieService = categoriesService;
        }

        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Index(int id = 1)
        {
            var page = Math.Max(1, id);

            var categories = await this.categorieService
                .GetAllAsync<CategoryCrudModel>(CategoryPerPage, (page - 1) * CategoryPerPage);

            var categoryCount = this.categorieService.GetCount();

            var pagesCount = (int)Math.Ceiling((decimal)categoryCount / CategoryPerPage);

            var viewModel = new CategoryCrudModelList
            {
                Categories = categories,
                PaginationModel = new PaginationViewModel
                {
                    CurrentPage = page,
                    TotalPages = pagesCount,
                    RouteName = "areaRoute",
                },
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Approve(int id)
        {
            var isApproved = await this.categorieService.ApproveCategoryAsync(id);

            if (!isApproved)
            {
                this.TempData[InvalidMessageKey] = InvalidApprovalMessage;
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
