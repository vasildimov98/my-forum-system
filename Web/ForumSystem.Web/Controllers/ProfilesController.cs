namespace ForumSystem.Web.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.PartialViews;
    using ForumSystem.Web.ViewModels.Posts;
    using ForumSystem.Web.ViewModels.Profiles;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using static ForumSystem.Common.GlobalConstants;

    public class ProfilesController : BaseController
    {
        private const int PostsPerPage = 5;

        private readonly IPostsService postsService;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;

        public ProfilesController(
            IPostsService postsService,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.postsService = postsService;
            this.environment = environment;
        }

        [Authorize]
        public async Task<IActionResult> ByUsername(string username, int id, string searchTerm)
        {
            if (id <= 0)
            {
                id = 1;

                this.TempData[InvalidMessageKey] = InvalidPageRequest;

                return this.RedirectToAction(nameof(this.ByUsername), new { username, id, searchTerm });
            }

            var currentPage = id;

            var loggedInUserId = this.userManager.GetUserId(this.User);

            var user = await this.userManager.Users
                .Include(x => x.Posts)
                .Include(x => x.ProfileImage)
                .FirstOrDefaultAsync(x => x.UserName == username);

            var posts = await this.postsService
                .GetAllByUserIdAsync<PostListViewModel>(
                user.Id,
                searchTerm,
                PostsPerPage,
                (currentPage - 1) * PostsPerPage);

            var postsCount = user.Posts.Count;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                postsCount = this.postsService
                    .GetCountByUsername(username, searchTerm);
            }

            var totalPages = (int)Math.Ceiling((double)postsCount / PostsPerPage);

            if (totalPages == 0)
            {
                totalPages = 1;
            }

            if (currentPage > totalPages)
            {
                currentPage = totalPages;

                this.TempData[InvalidMessageKey] = InvalidPageRequest;

                return this.RedirectToAction(nameof(this.ByUsername), new { username, id = currentPage, searchTerm });
            }

            var imageSrc = user.HasImage ?
                "/profileImages/" + user.ProfileImage.Id + user.ProfileImage.Extention :
                "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png";

            var viewModel = new ProfileViewModel
            {
                UserName = user.UserName,
                Posts = posts,
                PostsCount = postsCount,
                ImageSrc = imageSrc,
                CreatedOn = user.CreatedOn,
                IsLoggedInUser = user.Id == loggedInUserId,
                PaginationModel = new PaginationViewModel
                {
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    RouteName = "user-username-page",
                    SearchTerm = searchTerm,
                },
            };

            return this.View(viewModel);
        }
    }
}
