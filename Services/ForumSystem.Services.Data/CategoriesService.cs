namespace ForumSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Administration.Categories;

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

        public async Task<bool> CreateAsync(CategoryInputModel input)
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
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            return true;
        }

        public async Task EditAsync(int id, CategoryInputModel input)
        {
            var categoryToEdit = this.categoriesRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (categoryToEdit == null)
            {
                throw new ArgumentNullException(nameof(categoryToEdit));
            }

            categoryToEdit.Name = input.Name;
            categoryToEdit.Description = input.Description;
            categoryToEdit.ImageUrl = input.ImageUrl;

            this.categoriesRepository.Update(categoryToEdit);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var categoryToDelete = this.categoriesRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (categoryToDelete == null)
            {
                throw new ArgumentNullException(nameof(categoryToDelete));
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

        public async Task<IEnumerable<T>> GetAllAsync<T>(int? take = null, int skip = 0)
        {
            var query = this.categoriesRepository
                .All()
                .OrderByDescending(x => x.Posts.Count)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query
                    .Take(take.Value);
            }

            return await query
                .To<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
            => await this.categoriesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> GetMostFamousCategories<T>(int take = 5)
            => await this.categoriesRepository
                .All()
                .OrderByDescending(x => x.Posts.Count())
                .Take(take)
                .To<T>()
                .ToListAsync();

        public async Task<T> GetByNameAsync<T>(string name)
            => await this.categoriesRepository
            .All()
            .Where(x => x.Name == name)
            .To<T>()
            .FirstOrDefaultAsync();

        public int GetCount()
            => this.categoriesRepository
                   .All()
                   .Count();

        public int GetIdCategoryIdByName(string name)
            => this.categoriesRepository.All()
                .Where(x => x.Name == name)
                .Select(x => x.Id)
                .FirstOrDefault();
    }
}
