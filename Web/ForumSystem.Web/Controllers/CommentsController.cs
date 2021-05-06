namespace ForumSystem.Web.Controllers
{
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Comments;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("/api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsServer commentsServer;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsController(
            ICommentsServer commentsServer,
            UserManager<ApplicationUser> userManager)
        {
            this.commentsServer = commentsServer;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CommentResponseModel>> Post(int postId, string content)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var commentId = await this.commentsServer.AddAsync(postId, user.Id, content);

            // var comment = await this.commentsServer.GetByIdAsync(commentId);
            return new CommentResponseModel { Content = content };
        }
    }
}
