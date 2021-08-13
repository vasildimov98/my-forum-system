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

        public Task<IEnumerable<T>> GetAllAsync<T>(int? take = null, int skip = 0);

        public Task<IEnumerable<T>> GetAllByCategoryIdAsync<T>(int categoryId, int? take = null, int skip = 0);

        public Task<IEnumerable<T>> GetAllByUserIdAsync<T>(string userId, int? take = null, int skip = 0);

        public bool IsUserTheOwner(int postId, string userId);

        public T GetById<T>(int id);

        int GetCount();
    }
}
