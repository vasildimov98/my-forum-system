namespace ForumSystem.Web.Controllers.APIs
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Comments;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/comments")]
    public class CommentsApiController : BaseController
    {
        private readonly ICommentsService commentsServer;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsApiController(
            ICommentsService commentsServer,
            UserManager<ApplicationUser> userManager)
        {
            this.commentsServer = commentsServer;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostCommentAsync(CommentInputModel input)
        {
            var parentId = input.ParentId == 0 ?
                null :
                input.ParentId;

            if (parentId.HasValue)
            {
                if (!await this.commentsServer.IsInPostIdAsync(parentId.Value, input.PostId))
                {
                    return this.BadRequest();
                }
            }

            var userId = this.userManager.GetUserId(this.User);

            var commentViewModel = await this.commentsServer.AddAsync(input.PostId, userId, input.Content, parentId);

            return this.Ok(commentViewModel);
        }

        [HttpPost]
        [Authorize]
        [Route("edit")]
        public async Task<ActionResult> PutCommentAsync(EditCommentJsonModel editedInput)
        {
            try
            {
                var editViewModel = await this.commentsServer
                    .EditCommetAsync(editedInput.CommentId, editedInput.EditContent);

                return this.Ok(editViewModel);
            }
            catch
            {
                return this.BadRequest();
            }
        }
    }
}
