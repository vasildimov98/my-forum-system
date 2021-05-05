namespace ForumSystem.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels;
    using ForumSystem.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private IPostsService postService;

        public HomeController(IPostsService postService)
        {
            this.postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await this.postService.GetAllAsync<HomePostViewModel>();

            var postsList = new HomePostsListViewModel
            {
                Posts = posts
                .OrderByDescending(x => x.CreatedOn),
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
