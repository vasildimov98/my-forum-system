namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    public interface ICommentsServer
    {
        public Task<int> AddAsync(int postId, string id, string content);
    }
}
