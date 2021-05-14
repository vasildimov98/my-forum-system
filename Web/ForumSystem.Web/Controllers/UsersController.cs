namespace ForumSystem.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly IRepository<ProfileImage> profileImageRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(
            IWebHostEnvironment environment,
            IRepository<ProfileImage> profileImageRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.environment = environment;
            this.profileImageRepository = profileImageRepository;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromForm] IFormFile image)
        {
            var validExtention = new[] { ".png", ".jpeg", "jpg" };

            var profileImage = new ProfileImage();
            try
            {
                var fileName = image.FileName;

                if (image == null
                    || image.Length > 10 * 1024 * 1024)
                {
                    throw new InvalidOperationException("File is missing or its too big! Allowed length is 10MB");
                }

                var fileExtension = Path.GetExtension(fileName);

                if (!validExtention.Contains(fileExtension))
                {
                    throw new InvalidOperationException("Invalid file extention. Only png, jpeg, jpg are allowed");
                }

                var userId = this.userManager.GetUserId(this.User);

                var user = await this.userManager.Users
                    .Include(x => x.ProfileImage)
                    .FirstOrDefaultAsync(x => x.Id == userId);

                if (user.HasImage)
                {
                    System.IO.File.Delete($"{this.environment.WebRootPath}\\profileImages\\{user.ProfileImageId}{user.ProfileImage.Extention}");

                    this.profileImageRepository
                        .Delete(user.ProfileImage);

                    await this.profileImageRepository.SaveChangesAsync();
                }

                profileImage.Extention = fileExtension;
                profileImage.UserId = user.Id;

                user.ProfileImage = profileImage;
                user.HasImage = true;

                await this.userManager.UpdateAsync(user);
                await this.profileImageRepository.SaveChangesAsync();

                var filePathName = profileImage.Id + profileImage.Extention;

                var filePath = $"{this.environment.WebRootPath}\\profileImages\\";
                Directory.CreateDirectory(filePath);

                var physicalPath = $"{filePath}\\{filePathName}";
                using (var fs = new FileStream(physicalPath, FileMode.Create))
                {
                    await image.CopyToAsync(fs);
                    await fs.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }

            return this.Ok("/profileImages/" + profileImage.Id + profileImage.Extention);
        }
    }
}
