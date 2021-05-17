namespace ForumSystem.Web.Controllers
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
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(
            IUsersService usersService,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.environment = environment;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile image)
        {
            string imagePath;
            try
            {
                var userId = this.userManager.GetUserId(this.User);
                var filePath = Path.Combine(this.environment.WebRootPath, "profileImages");

                imagePath = await this.usersService.UploadProfileImage(image, userId, filePath);
            }
            catch (InvalidOperationException ioe)
            {
                return this.BadRequest(ioe.Message);
            }

            return this.Ok(imagePath);
        }

        [HttpPost]
        [Authorize]
        [Route("username")]
        public async Task<ActionResult<EditResponseModel>> Edit(EditUsernameInputModel input)
        {
            string username;
            var user = await this.userManager.GetUserAsync(this.User);
            try
            {
                username = await this.usersService.ChangeUsername(input.Username, user);
            }
            catch (InvalidOperationException ioe)
            {
                return new EditResponseModel { ErrorMessage = ioe.Message, Username = user.UserName };
            }

            return new EditResponseModel { Username = user.UserName };
        }
    }
}
