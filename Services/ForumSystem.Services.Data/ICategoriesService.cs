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

        public IEnumerable<T> GetAll<T>();

        public Task<T> GetByIdAsync<T>(int id);
    }
}
