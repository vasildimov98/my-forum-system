namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Administration.Categories;
    using ForumSystem.Web.ViewModels.PartialViews;

    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : AdministrationController
    {
        private const int CategoryPerPage = 5;

        private readonly ICategoriesService categorieService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categorieService = categoriesService;
        }

        public async Task<IActionResult> Index(int id)
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
                    PagesCount = pagesCount,
                    RouteName = "areaRoute",
                },
            };

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.categorieService.AddAsync(input);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await this.categorieService
                .GetByIdAsync<CategoryEditModel>(id);

            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Edit), new { id });
            }

            try
            {
                await this.categorieService.EditAsync(id, input);
            }
            catch
            {
                return this.NotFound();
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = await this.categorieService.GetByIdAsync<CategoryEditModel>((int)id);

            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await this.categorieService.DeleteAsync(id);
            }
            catch
            {
                return this.NotFound();
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
