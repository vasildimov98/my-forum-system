namespace ForumSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Categories;

    using Microsoft.EntityFrameworkCore;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IRepository<Comment> commentsRepository;

        public CategoriesService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<Post> postsRepository,
            IRepository<Comment> commentRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.postsRepository = postsRepository;
            this.commentsRepository = commentRepository;
        }

        public async Task<bool> CreateAsync(CategoryInputModel input, string userId, bool isUserAdmin = false)
        {
            var isCategoryNameTaken = this.categoriesRepository
                .All()
                .Any(x => x.Name == input.Name);

            if (isCategoryNameTaken)
            {
                return false;
            }

            var category = new Category
            {
                Name = input.Name,
                Description = input.Description,
                ImageUrl = input.ImageUrl,
                OwnerId = userId,
                IsApprovedByAdmin = isUserAdmin,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            return true;
        }

        public async Task EditAsync(bool isUserAdmin, string userId, CategoryEditModel input)
        {
            var categoryToEdit = this.categoriesRepository
                .All()
                .FirstOrDefault(x => x.Id == input.Id);

            if (categoryToEdit == null)
            {
                throw new InvalidOperationException(nameof(categoryToEdit));
            }

            if (!isUserAdmin)
            {
                var isUserTheOwner = this.IsUserTheOwner(input.Id, userId);

                if (!isUserTheOwner)
                {
                    throw new UnauthorizedAccessException();
                }
            }

            categoryToEdit.Name = input.Name;
            categoryToEdit.Description = input.Description;
            categoryToEdit.ImageUrl = input.ImageUrl;
            categoryToEdit.IsApprovedByAdmin = false;

            this.categoriesRepository.Update(categoryToEdit);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(bool isUserAdmin, string userId, int categoryId)
        {
            var categoryToDelete = this.categoriesRepository
                .All()
                .FirstOrDefault(x => x.Id == categoryId);

            if (categoryToDelete == null)
            {
                throw new InvalidOperationException();
            }

            if (!isUserAdmin)
            {
                var isUserTheOwner = this.IsUserTheOwner(categoryId, userId);

                if (!isUserTheOwner)
                {
                    throw new UnauthorizedAccessException();
                }
            }

            var posts = this.postsRepository
                .All()
                .Where(x => x.CategoryId == categoryToDelete.Id)
                .ToList();

            foreach (var post in posts)
            {
                this.postsRepository.Delete(post);

                var comments = this.commentsRepository.All()
                    .Where(x => x.PostId == post.Id)
                    .ToList();

                foreach (var comment in comments)
                {
                    this.commentsRepository.Delete(comment);
                }
            }

            this.categoriesRepository.Delete(categoryToDelete);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(
            string searchTerm = null,
            int? take = null,
            int skip = 0,
            bool onlyApproved = true)
        {
            var query = this.categoriesRepository
                .All();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query
                    .Where(x => EF.Functions.Like(x.Name, $"%{searchTerm}%")
                    || EF.Functions.Like(x.Description, $"%{searchTerm}%"));
            }

            if (onlyApproved)
            {
                query = query
                    .Where(x => x.IsApprovedByAdmin)
                    .OrderByDescending(x => x.Posts.Count)
                    .Skip(skip);
            }

            if (!onlyApproved)
            {
                query = query
                    .OrderBy(x => x.IsApprovedByAdmin)
                    .ThenBy(x => x.CreatedOn)
                    .Skip(skip);
            }

            if (take.HasValue)
            {
                query = query
                    .Take(take.Value);
            }

            return await query
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByOwnerUsernameAsync<T>(
            string username,
            string searchTerm = null,
            int? take = null,
            int skip = 0)
        {
            var query = this.categoriesRepository
                    .All()
                    .Where(x => x.Owner.UserName == username && x.IsApprovedByAdmin)
                    .OrderByDescending(x => x.Posts.Count)
                    .Skip(skip);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query
                    .Where(x => EF.Functions.Like(x.Name, $"%{searchTerm}%")
                        || EF.Functions.Like(x.Description, $"%{searchTerm}%"));
            }

            if (take.HasValue)
            {
                query = query
                    .Take(take.Value);
            }

            return await query
                .To<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int categoryId)
            => await this.categoriesRepository
                .All()
                .Where(x => x.Id == categoryId)
                .To<T>()
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> GetMostFamousCategories<T>(int take = 5)
            => await this.categoriesRepository
                .All()
                .Where(x => x.IsApprovedByAdmin)
                .OrderByDescending(x => x.Posts.Count())
                .Take(take)
                .To<T>()
                .ToListAsync();

        public async Task<T> GetByNameAsync<T>(string name)
            => await this.categoriesRepository
            .All()
            .Where(x => x.Name == name && x.IsApprovedByAdmin)
            .To<T>()
            .FirstOrDefaultAsync();

        public int GetCount(string searchTerm = null, bool onlyApproved = true)
        {
            var query = this.categoriesRepository
                .All();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query
                    .Where(x => EF.Functions.Like(x.Name, $"%{searchTerm}%")
                    || EF.Functions.Like(x.Description, $"%{searchTerm}%"));
            }

            if (onlyApproved)
            {
                query = query
                   .Where(x => x.IsApprovedByAdmin);
            }

            return query
                .Count();
        }

        public int GetCountByOwner(string username, string searchTerm = null)
        {
            var query = this.categoriesRepository
                .All();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query
                    .Where(x => EF.Functions.Like(x.Name, $"%{searchTerm}%")
                    || EF.Functions.Like(x.Description, $"%{searchTerm}%"));
            }

            return query
                .Where(x => x.IsApprovedByAdmin && x.Owner.UserName == username)
                .Count();
        }

        public int GetIdCategoryIdByName(string name)
            => this.categoriesRepository
                .All()
                .Where(x => x.Name == name)
                .Select(x => x.Id)
                .FirstOrDefault();

        public async Task<bool> ApproveCategoryAsync(int categoryId)
        {
            var category = this.categoriesRepository
                 .All()
                 .Where(x => x.Id == categoryId)
                 .FirstOrDefault();

            if (category == null)
            {
                return false;
            }

            category.IsApprovedByAdmin = true;
            await this.categoriesRepository.SaveChangesAsync();

            return category.IsApprovedByAdmin;
        }

        public bool IsUserTheOwner(int categoryId, string userId)
            => this.categoriesRepository
                .All()
                .Any(x => x.OwnerId == userId && x.Id == categoryId);

        public async Task<IEnumerable<T>> FindCategoryByTermSearchAsync<T>(string term)
            => await this.categoriesRepository
                .All()
                .Where(x => EF.Functions.Like(x.Name, $"%{term}%")
                || EF.Functions.Like(x.Description, $"%{term}%"))
                .To<T>()
                .ToListAsync();
    }
}
