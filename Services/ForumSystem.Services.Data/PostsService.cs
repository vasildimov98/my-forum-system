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

        public async Task EditAsync(
            bool isUserAdmin,
            string userId,
            int postId,
            string title,
            string content,
            int categoryId)
        {
            var postToEdit = await this.postsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == postId);

            if (postToEdit == null)
            {
                throw new InvalidOperationException(nameof(postToEdit));
            }

            if (!isUserAdmin)
            {
                var isUserTheOwner = this.IsUserTheOwner(postId, userId);

                if (!isUserTheOwner)
                {
                    throw new UnauthorizedAccessException();
                }
            }

            postToEdit.Title = title;
            postToEdit.Content = content;
            postToEdit.CategoryId = categoryId;

            this.postsRepository.Update(postToEdit);
            await this.postsRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(bool isUserAdmin, string userId, int postId)
        {
            var post = await this.postsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == postId);

            if (post == null)
            {
                throw new InvalidOperationException("Post doesn't exits");
            }

            if (!isUserAdmin)
            {
                var isUserTheOwner = this.IsUserTheOwner(postId, userId);

                if (!isUserTheOwner)
                {
                    throw new UnauthorizedAccessException();
                }
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

        public async Task<IEnumerable<T>> GetAllAsync<T>(
            string searchTerm = null,
            int? take = null,
            int skip = 0)
        {
            var postsQuery = this.postsRepository
                .All()
                .OrderByDescending(x => x.Comments.Count)
                .ThenByDescending(x => x.CreatedOn)
                .Skip(skip);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                postsQuery = FilterSearchTerm(searchTerm, postsQuery);
            }

            if (take.HasValue)
            {
                postsQuery = postsQuery
                    .Take(take.Value);
            }

            return await postsQuery
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByCategoryIdAsync<T>(
            int categoryId,
            string searchTerm = null,
            int? take = null,
            int skip = 0)
        {
            var postsQuery = this.postsRepository
               .All()
               .Where(x => x.CategoryId == categoryId)
               .OrderByDescending(x => x.Comments.Count)
               .ThenByDescending(x => x.CreatedOn)
               .Skip(skip);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                postsQuery = FilterSearchTerm(searchTerm, postsQuery);
            }

            if (take.HasValue)
            {
                postsQuery = postsQuery
                    .Take(take.Value);
            }

            return await postsQuery
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByUserIdAsync<T>(
            string userId,
            string searchTerm,
            int? take = null,
            int skip = 0)
        {
            var postsQuery = this.postsRepository
               .All()
               .Where(x => x.UserId == userId)
               .OrderByDescending(x => x.Comments.Count)
               .ThenByDescending(x => x.CreatedOn)
               .Skip(skip);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                postsQuery = FilterSearchTerm(searchTerm, postsQuery);
            }

            if (take.HasValue)
            {
                postsQuery = postsQuery
                    .Take(take.Value);
            }

            return await postsQuery
                .To<T>()
                .ToListAsync();
        }

        public T GetById<T>(int id)
            => this.postsRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

        public T GetByIdAndTitle<T>(int id, string title)
            => this.postsRepository
                .All()
                .Where(x => x.Id == id
                && EF.Functions.Like(x.Title, $"%{title}%"))
                .To<T>()
                .FirstOrDefault();

        public int GetCount(string searchTerm = null)
        {
            var postsQuery = this.postsRepository.All();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                postsQuery = FilterSearchTerm(searchTerm, postsQuery);
            }

            return postsQuery.Count();
        }

        public int GetCountByCategoryName(string categoryName, string searchTerm)
            => this.postsRepository
                   .All()
                   .Where(x => (x.Category.Name == categoryName)
                         && (EF.Functions.Like(x.Title, $"%{searchTerm}%")
                            || EF.Functions.Like(x.Content, $"%{searchTerm}%")
                            || EF.Functions.Like(x.Category.Name, $"%{searchTerm}%")
                            || EF.Functions.Like(x.Category.Description, $"%{searchTerm}%")))
                   .Count();

        public int GetCountByUsername(string username, string searchTerm)
             => this.postsRepository
                   .All()
                   .Where(x => (x.User.UserName == username)
                         && (EF.Functions.Like(x.Title, $"%{searchTerm}%")
                            || EF.Functions.Like(x.Content, $"%{searchTerm}%")
                            || EF.Functions.Like(x.Category.Name, $"%{searchTerm}%")
                            || EF.Functions.Like(x.Category.Description, $"%{searchTerm}%")))
                   .Count();

        public bool IsUserTheOwner(int postId, string userId)
            => this.postsRepository
                .All()
                .Any(x => x.Id == postId && x.UserId == userId);

        private static IQueryable<Post> FilterSearchTerm(string searchTerm, IQueryable<Post> postsQuery)
            => postsQuery
              .Where(x => EF.Functions.Like(x.Title, $"%{searchTerm}%")
                       || EF.Functions.Like(x.Content, $"%{searchTerm}%")
                       || EF.Functions.Like(x.Category.Name, $"%{searchTerm}%")
                       || EF.Functions.Like(x.Category.Description, $"%{searchTerm}%"));
    }
}
