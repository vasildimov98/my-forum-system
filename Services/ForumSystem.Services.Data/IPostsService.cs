namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostsService
    {
        Task<int> CreateAsync(string title, string content, int categoryId, string userId);

        Task EditAsync(
            bool isUserAdmin,
            string userId,
            int postId,
            string title,
            string content,
            int categoryId);

        Task DeleteAsync(bool isUserAdmin, string userId, int postId);

        public Task<IEnumerable<T>> GetAllAsync<T>(
            string searchTerm = null,
            int? take = null,
            int skip = 0);

        public Task<IEnumerable<T>> GetAllByCategoryIdAsync<T>(
            int categoryId,
            string searchTerm = null,
            int? take = null,
            int skip = 0);

        public Task<IEnumerable<T>> GetAllByUserIdAsync<T>(string userId, string searchTerm, int? take = null, int skip = 0);

        public bool IsUserTheOwner(int postId, string userId);

        public T GetById<T>(int id);

        int GetCount(string searchTerm = null);

        int GetCountByCategoryName(string categoryName, string searchTerm);
        int GetCountByUsername(string username, string searchTerm);
    }
}
