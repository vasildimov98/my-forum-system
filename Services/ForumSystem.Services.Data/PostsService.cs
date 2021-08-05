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
        private readonly IRepository<Comment> commentsRepository;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository,
            IRepository<Comment> commentsRepository)
        {
            this.postsRepository = postsRepository;
            this.commentsRepository = commentsRepository;
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

        public async Task DeleteAsync(int postId)
        {
            var post = await this.postsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == postId);

            if (post == null)
            {
                throw new InvalidOperationException("Post doesn't exits");
            }

            var comments = await this.commentsRepository.All()
                   .Where(x => x.PostId == post.Id)
                   .ToListAsync();

            foreach (var comment in comments)
            {
                this.commentsRepository.Delete(comment);
            }

            this.postsRepository.Delete(post);

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
