namespace ForumSystem.Web.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;

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
    }
}
