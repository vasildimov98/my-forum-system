namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostsService
    {
        Task<int> CreateAsync(string title, string content, int categoryId, string userId);

        public Task<IEnumerable<T>> GetAllAsync<T>();

        public Task<T> GetByIdAsync<T>(int id);
    }
}
