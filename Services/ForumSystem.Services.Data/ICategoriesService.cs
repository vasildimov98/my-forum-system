namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.ViewModels.Administration.Categories;

    public interface ICategoriesService
    {
        public Task AddAsync(CategoryInputModel input);

        public Task EditAsync(int id, CategoryInputModel input);

        public IEnumerable<CategoryCrudModel> GetAll();

        public CategoryEditModel GetById(int id);
    }
}
