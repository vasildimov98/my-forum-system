namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
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
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.environment = environment;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public string Post([FromForm]IFormFile files)
        {
            string result;
            try
            {
                long size = 0;
                var file = this.Request.Form.Files;
                var filename = ContentDispositionHeaderValue
                                .Parse(file[0].ContentDisposition).FileName
                                .Trim('"');

                string filePath = this.environment.WebRootPath + $@"/{filename}";

                size += file[0].Length;

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file[0].CopyTo(fs);
                    fs.Flush();
                }

                result = filePath;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
    }
}
