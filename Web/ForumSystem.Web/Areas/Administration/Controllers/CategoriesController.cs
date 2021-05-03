namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data;
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
            var categories = this.categorieService.GetAll();

            return this.View(categories);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.categorieService.AddAsync(input);

            return this.RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var category = this.categorieService.GetById(id);

            return this.View(category);
        }

        [HttpPost]
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

            return this.RedirectToAction("Index");
        }
    }
}
