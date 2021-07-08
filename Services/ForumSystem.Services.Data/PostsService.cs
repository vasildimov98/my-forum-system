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

    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository)
        {
            this.postsRepository = postsRepository;
        }

        public async Task<int> CreateAsync(string title, string content, int categoryId, string userId)
        {
            var post = new Post
            {
                Title = title,
                Content = content,
                CategoryId = categoryId,
                UserId = userId,
            };

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();

            return post.Id;
        }

        public async Task EditAsync(int postId, string title, string content, int categoryId)
        {
            var postToEdit = await this.postsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == postId);

            if (postToEdit == null)
            {
                throw new ArgumentNullException(nameof(postToEdit));
            }

            postToEdit.Title = title;
            postToEdit.Content = content;
            postToEdit.CategoryId = categoryId;

            this.postsRepository.Update(postToEdit);
            await this.postsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(int? take = null, int skip = 0)
        {
            var postsQuery = this.postsRepository
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .Skip(skip);

            if (take.HasValue)
            {
                postsQuery = postsQuery
                    .Take(take.Value);
            }

            return await postsQuery
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByCategoryIdAsync<T>(int categoryId, int? take = null, int skip = 0)
        {
            var postsQuery = this.postsRepository
               .All()
               .Where(x => x.CategoryId == categoryId)
               .OrderByDescending(x => x.CreatedOn)
               .Skip(skip);

            if (take.HasValue)
            {
                postsQuery = postsQuery
                    .Take(take.Value);
            }

            return await postsQuery
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByUserIdAsync<T>(string userId, int? take = null, int skip = 0)
        {
            var postsQuery = this.postsRepository
               .All()
               .Where(x => x.UserId == userId)
               .OrderByDescending(x => x.CreatedOn)
               .Skip(skip);

            if (take.HasValue)
            {
                postsQuery = postsQuery
                    .Take(take.Value);
            }

            return await postsQuery
                .To<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
            => await this.postsRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

        public int GetCount()
            => this.postsRepository
                .All()
                .Count();
    }
}
