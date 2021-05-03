namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.ViewModels.Administration.Categories;

    public interface ICategoriesService
    {
        public IEnumerable<CategoryCrudModel> GetAll();

        public Task AddAsync(CategoryInputModel input);
    }
}
