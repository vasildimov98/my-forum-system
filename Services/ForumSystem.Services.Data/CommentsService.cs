namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Data.Common.Repositories;

    public class CommentsService : ICommentsServer
    {
        private readonly IDeletableEntityRepository<Comment> comments;

        public CommentsService(IDeletableEntityRepository<Comment> comments)
        {
            this.comments = comments;
        }

        public async Task<int> AddAsync(int postId, string userId, string content, int? parentId = null)
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

            return comment.Id;
        }
    }
}
