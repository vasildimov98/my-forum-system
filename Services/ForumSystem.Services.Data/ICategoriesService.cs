namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ForumSystem.Web.ViewModels.Administration.Categories;

    public interface ICategoriesService
    {
        public Task AddAsync(CategoryInputModel input);

        public Task EditAsync(int id, CategoryInputModel input);

        public Task DeleteAsync(int id);

        public Task<IEnumerable<T>> GetAllAsync<T>(int? take = null, int skip = 0);

        public Task<IEnumerable<T>> GetMostPostsCategories<T>(int take = 5);

        public Task<T> GetByIdAsync<T>(int id);

        public Task<T> GetByNameAsync<T>(string name);

        public bool ValidateCategoryName(string name);

        public int GetCount();
    }
}
