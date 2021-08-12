namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ForumSystem.Web.ViewModels.Categories;

    public interface ICategoriesService
    {
        public Task<bool> CreateAsync(CategoryInputModel input, string userId, bool isUserAdmin = false);

        public Task EditAsync(int id, CategoryEditModel input);

        public Task DeleteAsync(int id);

        public Task<IEnumerable<T>> GetAllAsync<T>(int? take = null, int skip = 0, bool onlyApproved = true);

        public Task<IEnumerable<T>> GetByOwnerUsernameAsync<T>(string userId, int? take = null, int skip = 0);

        public Task<IEnumerable<T>> GetMostFamousCategories<T>(int take = 5);

        public Task<T> GetByIdAsync<T>(int id);

        public Task<T> GetByNameAsync<T>(string name);

        Task<bool> ApproveCategoryAsync(int id);

        public int GetCount(bool onlyApproved = true);

        int GetCountByOwner(string username);

        public int GetIdCategoryIdByName(string name);
    }
}
