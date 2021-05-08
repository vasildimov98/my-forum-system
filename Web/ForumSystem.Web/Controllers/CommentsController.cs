﻿namespace ForumSystem.Web.Controllers
{
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Comments;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<CommentInputModel>> Create(CommentInputModel input)
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

            await this.commentsServer.AddAsync(input.PostId, userId, input.Content, parentId);

            return this.RedirectToAction("ById", "Posts", new { id = input.PostId });
        }
    }
}