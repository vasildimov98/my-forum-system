namespace ForumSystem.Web.Controllers
{
    using System.Diagnostics;

    using ForumSystem.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        public IActionResult Privacy()
            => this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => this.View();
    }
}
