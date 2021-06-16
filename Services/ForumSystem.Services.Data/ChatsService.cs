namespace ForumSystem.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;

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

        public async Task CreateMessageAsync(string categoryName, string userId, string content)
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
        }
    }
}
