namespace ForumSystem.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Comments;
    using Microsoft.EntityFrameworkCore;

    public class CommentsService : ICommentsService
    {
        private readonly IRepository<Comment> comments;

        public CommentsService(
            IRepository<Comment> comments)
        {
            this.comments = comments;
        }

        public async Task<PostCommentViewModel> AddAsync(int postId, string userId, string content, int? parentId = null)
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

            var commentViewModel = this.comments
                .All()
                .Where(x => x.Id == comment.Id)
                .To<PostCommentViewModel>()
                .FirstOrDefault();

            return commentViewModel;
        }

        public async Task<string> DeleteCommentAsync(int commentId)
        {
            var comment = this.comments
                .All()
                .Include(x => x.SubComments)
                .Where(x => x.Id == commentId)
                .FirstOrDefault();

            if (comment == null)
            {
                throw new InvalidOperationException();
            }

            this.RemoveSubCommentsComment(comment);
            this.comments
                .Delete(comment);

            await this.comments.SaveChangesAsync();

            return "Delete is successfull";
        }

        public async Task<EditCommentViewModel> EditCommetAsync(int commentId, string content)
        {
            var commentToEdit = await this.comments
                .All()
                .Where(x => x.Id == commentId)
                .FirstOrDefaultAsync();

            if (commentToEdit == null)
            {
                throw new InvalidOperationException("CommentId is invalid!");
            }

            commentToEdit.Content = content;

            await this.comments.SaveChangesAsync();

            return new EditCommentViewModel
            {
                Content = commentToEdit.Content,
            };
        }

        public string GetCommentContent(int commentId)
            => this.comments
                .All()
                .Where(x => x.Id == commentId)
                .Select(x => x.Content)
                .FirstOrDefault();

        public async Task<bool> IsInPostIdAsync(int commentId, int postId)
        {
            var commentPostId = await this.comments
                .All()
                .Where(x => x.Id == commentId)
                .Select(x => x.PostId)
                .FirstOrDefaultAsync();

            return commentPostId == postId;
        }

        public bool IsSignInUserTheOwenerOfComment(int commentId, string userId)
            => this.comments
                .All()
                .Any(x => x.Id == commentId && x.UserId == userId);

        private void RemoveSubCommentsComment(Comment comment)
        {
            if (comment.SubComments.Count == 0)
            {
                this.comments.Delete(comment);
                return;
            }

            foreach (var subComment in comment.SubComments)
            {
                var includedSubComments = this.comments.All()
                    .Include(x => x.SubComments)
                    .FirstOrDefault(x => x.Id == subComment.Id);

                this.RemoveSubCommentsComment(includedSubComments);
            }

            this.comments.Delete(comment);
        }
    }
}
