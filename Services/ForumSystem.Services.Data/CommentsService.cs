﻿namespace ForumSystem.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class CommentsService : ICommentsServer
    {
        private readonly IDeletableEntityRepository<Comment> comments;

        public CommentsService(IDeletableEntityRepository<Comment> comments)
        {
            this.comments = comments;
        }

        public async Task AddAsync(int postId, string userId, string content, int? parentId = null)
        {
            var comment = new Comment
            {
                PostId = postId,
                UserId = userId,
                Content = content,
                ParentId = parentId,
            };

            await this.comments.AddAsync(comment);
            await this.comments.SaveChangesAsync();
        }

        public async Task<bool> IsInPostIdAsync(int commentId, int postId)
        {
            var commentPostId = await this.comments
                .All()
                .Where(x => x.Id == commentId)
                .Select(x => x.PostId)
                .FirstOrDefaultAsync();

            return commentPostId == postId;
        }
    }
}
