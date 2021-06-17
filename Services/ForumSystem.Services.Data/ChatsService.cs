namespace ForumSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class ChatsService : IChatsService
    {
        private readonly IRepository<Message> messageRepository;
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public ChatsService(
            IRepository<Message> messageRepository,
            IDeletableEntityRepository<Category> categoryRepository)
        {
            this.messageRepository = messageRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<DateTime> CreateMessageAsync(string categoryName, string userId, string content)
        {
            categoryName = categoryName
                .Replace("-", " ");

            var categoryId = this.categoryRepository
                .All()
                .Where(x => x.Name == categoryName)
                .Select(x => x.Id)
                .FirstOrDefault();

            if (categoryId == 0)
            {
                throw new InvalidOperationException("Invalid category name!");
            }

            var message = new Message
            {
                CategoryId = categoryId,
                UserId = userId,
                Content = content,
            };

            await this.messageRepository.AddAsync(message);
            await this.messageRepository.SaveChangesAsync();

            return message.CreatedOn;
        }

        public async Task<IEnumerable<T>> GetAllMessagesByCategoryId<T>(int categoryId)
            => await this.messageRepository
                .All()
                .Where(x => x.CategoryId == categoryId)
                .To<T>()
                .ToListAsync();
    }
}
