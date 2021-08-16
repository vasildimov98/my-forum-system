namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ForumSystem.Web.ViewModels.Categories;

    public interface ICategoriesService
    {
        public Task<bool> CreateAsync(CategoryInputModel input, string userId, bool isUserAdmin = false);

        public Task EditAsync(bool isUserAdmin, string userId, CategoryEditModel input);

        public Task DeleteAsync(bool isUserAdmin, string userId, int categoryId);

        public Task<IEnumerable<T>> GetAllAsync<T>(string searchTerm = null, int? take = null, int skip = 0, bool onlyApproved = true);

        public Task<IEnumerable<T>> GetByOwnerUsernameAsync<T>(string username, string searchTerm = null, int? take = null, int skip = 0);

        Task<IEnumerable<T>> FindCategoryByTermSearchAsync<T>(string term);

        public Task<IEnumerable<T>> GetMostFamousCategories<T>(int take = 5);

        public Task<T> GetByIdAsync<T>(int categoryId);

        public Task<T> GetByNameAsync<T>(string name);

        Task<bool> ApproveCategoryAsync(int categoryId);

        public int GetCount(string searchTerm = null, bool onlyApproved = true);

        int GetCountByOwner(string username, string searchTerm = null);

        public int GetIdCategoryIdByName(string name);

        bool IsUserTheOwner(int categoryId, string userId);
    }
}
