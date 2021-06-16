namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    public interface IChatsService
    {
        Task CreateMessageAsync(string categoryName, string userId, string content);
    }
}
