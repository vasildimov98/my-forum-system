namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.PartialViews;
    using ForumSystem.Web.ViewModels.Profiles;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class ProfilesController : BaseController
    {
        private const int PostsPerPage = 5;

        private readonly IPostsService postsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ProfilesController(
            IPostsService postsService,
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.postsService = postsService;
        }

        [Authorize]
        public async Task<IActionResult> All(int id)
        {
            var page = Math.Max(1, id);

            var userId = this.userManager.GetUserId(this.User);

            var user = await this.userManager.Users
                .Include(x => x.Posts)
                .FirstOrDefaultAsync(x => x.Id == userId);

            var posts = await this.postsService
                .GetAllByUserIdAsync<ProfilePostsViewModel>(user.Id, PostsPerPage, (page - 1) * PostsPerPage);

            var postsCount = user.Posts.Count;

            var pagesCount = (int)Math.Ceiling((double)postsCount / PostsPerPage);

            var viewModel = new ProfileViewModel
            {
                UserName = user.UserName,
                Posts = posts,
                PostsCount = postsCount,
                PaginationModel = new PaginationViewModel
                {
                    CurrentPage = page,
                    PagesCount = pagesCount,
                    RouteName = "default",
                },
            };

            return this.View(viewModel);
        }
    }
}
