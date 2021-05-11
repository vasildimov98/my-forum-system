﻿namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Categories;
    using ForumSystem.Web.ViewModels.PartialViews;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : BaseController
    {
        private const int PostsPerPage = 5;

        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;

        public CategoriesController(
            ICategoriesService categoriesService,
            IPostsService postsService)
        {
            this.categoriesService = categoriesService;
            this.postsService = postsService;
        }

        public async Task<IActionResult> ByName(string name, int id)
        {
            var page = id;

            name = name.Replace("-", " ");

            var category = await this.categoriesService
                .GetByNameAsync<CategoryPostsListModel>(name);

            if (category == null)
            {
                return this.NotFound();
            }

            category.Posts = await this.postsService
                .GetAllByCategoryIdAsync<CategoryPostViewModel>(category.Id, PostsPerPage, (page - 1) * PostsPerPage);

            var pagesCount = (int)Math.Ceiling((double)category.PostsCount / PostsPerPage);

            category.PaginationModel = new PaginationViewModel
            {
                TotalPosts = category.PostsCount,
                CurrentPage = page,
                PagesCount = pagesCount,
                RouteName = "category-name-page",
            };

            return this.View(category);
        }
    }
}