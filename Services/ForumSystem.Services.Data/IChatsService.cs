namespace ForumSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IChatsService
    {
        Task<DateTime> CreateMessageAsync(string categoryName, string userId, string content);

        Task<IEnumerable<T>> GetAllMessagesByCategoryId<T>(int categoryId);
    }
}
