namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Administration.Categories;

    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : AdministrationController
    {
        private readonly ICategoriesService categorieService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categorieService = categoriesService;
        }

        public IActionResult Index()
        {
            var categories = this.categorieService.GetAll<CategoryCrudModel>();

            return this.View(categories);
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
            var category = await this.categorieService.GetByIdAsync<CategoryViewModel>(id);

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
                return this.View(id);
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

            var category = await this.categorieService.GetByIdAsync<CategoryViewModel>((int)id);

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
