namespace ForumSystem.Services.Data
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly string[] validExtention = new[] { ".png", ".jpeg", ".jpg" };
        private readonly Regex usernameRegex = new("^(?=.{7,20}$)[a-zA-Z0-9]+$");

        private readonly IRepository<ProfileImage> profileImages;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(
            IRepository<ProfileImage> profileImages,
            UserManager<ApplicationUser> userManager)
        {
            this.profileImages = profileImages;
            this.userManager = userManager;
        }

        public async Task<string> ChangeUsername(string username, ApplicationUser user)
        {
            if (!this.usernameRegex.IsMatch(username))
            {
                throw new InvalidOperationException("Invalid username." +
                    " Only alphanumeric character." +
                    " Min Length: 7." +
                    " Max Length: 20");
            }

            var userWithSameuserName = await this.userManager.Users
                .Where(x => x.UserName == username
                    && x.Id != user.Id)
                .FirstOrDefaultAsync();

            if (userWithSameuserName != null)
            {
                throw new InvalidOperationException("This username is taken. Try another one!");
            }

            user.UserName = username;

            await this.userManager.UpdateAsync(user);

            return user.UserName;
        }

        public async Task<string> UploadProfileImage(IFormFile image, string userId, string path)
        {
            if (image == null
                || image.Length > 10 * 1024 * 1024)
            {
                throw new InvalidOperationException("File is missing or its too big! Allowed length is 10MB");
            }

            var fileName = image.FileName;
            var fileExtension = Path.GetExtension(fileName);

            if (!this.validExtention.Contains(fileExtension))
            {
                throw new InvalidOperationException("Invalid file extention. Only png, jpeg, jpg are allowed");
            }

            var user = await this.userManager.Users
                .Include(x => x.ProfileImage)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user.HasImage)
            {
                var oldProfileImagePath = Path.Combine(path, user.ProfileImageId + user.ProfileImage.Extention);

                File
                    .Delete(oldProfileImagePath);

                this.profileImages
                    .Delete(user.ProfileImage);

                await this.profileImages.SaveChangesAsync();
            }

            var profileImage = new ProfileImage
            {
                Extention = fileExtension,
                UserId = userId,
            };

            user.ProfileImage = profileImage;
            user.HasImage = true;

            await this.userManager.UpdateAsync(user);
            await this.profileImages.SaveChangesAsync();

            var filePathName = profileImage.Id + profileImage.Extention;

            Directory.CreateDirectory(path);

            var physicalPath = Path.Combine(path, filePathName);

            using var fs = new FileStream(physicalPath, FileMode.Create);

            await image.CopyToAsync(fs);
            await fs.FlushAsync();

            return $"/profileImages/{filePathName}";
        }
    }
}
