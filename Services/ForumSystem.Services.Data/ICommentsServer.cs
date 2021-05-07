namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    public interface ICommentsServer
    {
        public Task AddAsync(int postId, string userId, string content, int? parentId);

        public Task<bool> IsInPostIdAsync(int commentId, int postId);
    }
}
