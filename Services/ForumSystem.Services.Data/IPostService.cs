namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostService
    {
        Task<int> CreateAsync(string title, string content, int categoryId, string userId);

        public Task<IEnumerable<T>> GetAll<T>();

        public Task<T> GetByIdAsync<T>(int id);
    }
}
