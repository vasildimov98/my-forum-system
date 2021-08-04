namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    using ForumSystem.Web.ViewModels.Comments;

    public interface ICommentsService
    {
        public Task<PostCommentViewModel> AddAsync(int postId, string userId, string content, int? parentId);

        public Task<EditCommentViewModel> EditCommetAsync(int commentId, string content);

        public Task<bool> IsInPostIdAsync(int commentId, int postId);

        public bool IsSignInUserTheOwenerOfComment(int commentId, string userId);

        string GetCommentContent(int commentId);

        Task<string> DeleteCommentAsync(int commentId);
    }
}
