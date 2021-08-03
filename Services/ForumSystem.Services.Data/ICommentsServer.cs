namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;
    using ForumSystem.Web.ViewModels.Comments;

    public interface ICommentsServer
    {
        public Task<PostCommentViewModel> AddAsync(int postId, string userId, string content, int? parentId);

        public Task<bool> IsInPostIdAsync(int commentId, int postId);
    }
}
