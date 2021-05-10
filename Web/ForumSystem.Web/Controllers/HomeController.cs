﻿namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels;
    using ForumSystem.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private const int PostsPerPage = 5;

        private IPostsService postService;

        public HomeController(IPostsService postService)
        {
            this.postService = postService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var posts = await this.postService
                .GetAllAsync<HomePostViewModel>(PostsPerPage, (page - 1) * PostsPerPage);

            var count = this.postService.GetCount();
            var pageCount = (int)Math.Ceiling((double)count / PostsPerPage);
            var postsList = new HomePostsListViewModel
            {
                Posts = posts,
                CurrentPage = page,
                PagesCount = pageCount,
            };

            return this.View(postsList);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
