namespace ForumSystem.Web.Controllers.APIs
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersApiController(
            IWebHostEnvironment environment,
            IUsersService usersService,
            UserManager<ApplicationUser> userManager)
        {
            this.environment = environment;
            this.usersService = usersService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        [Route("username")]
        public async Task<ActionResult<EditResponseModel>> EditUsername(EditUsernameInputModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            string username;

            try
            {
                username = await this.usersService.ChangeUsername(input.Username, user);
            }
            catch (InvalidOperationException ioe)
            {
                return new EditResponseModel
                {
                    ErrorMessage = ioe.Message,
                    Username = user.UserName,
                };
            }

            return new EditResponseModel
            {
                Username = username,
            };
        }

        [HttpPost]
        [Authorize]
        [Route("image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile image)
        {
            string imagePath;
            try
            {
                var userId = this.userManager.GetUserId(this.User);

                var filePath = Path.Combine(this.environment.WebRootPath, "profileImages");

                Directory.CreateDirectory(filePath);

                imagePath = await this.usersService.UploadProfileImage(image, userId, filePath);
            }
            catch (InvalidOperationException ioe)
            {
                return this.BadRequest(ioe.Message);
            }

            return this.Ok(imagePath);
        }
    }
}
