namespace ForumSystem.Web.Tests.Routes.APIs
{
    using ForumSystem.Web.Controllers.APIs;
    using ForumSystem.Web.ViewModels.Comments;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class CommentsApiControllerTests
    {
        [Fact]
        public void GetCommentContentShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/api/comments?commentId=1")
                .To<CommentsApiController>(c => c.GetCommentContent(With.Value<int>(1)));

        [Fact]
        public void PostCommentAsyncShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/api/comments")
                    .WithJsonBody(new
                    {
                        postId = 2,
                        parentId = 1,
                        content = "test content",
                    }))
                .To<CommentsApiController>(c => c.PostCommentAsync(new CommentInputModel
                {
                    PostId = 2,
                    ParentId = 1,
                    Content = "test content",
                }));

        [Fact]
        public void PutCommentAsyncShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Put)
                    .WithLocation("/api/comments/edit")
                    .WithJsonBody(new
                    {
                        commentId = 1,
                        editContent = "tested content edited",
                    }))
                .To<CommentsApiController>(c => c.PutCommentAsync(new EditCommentJsonModel
                {
                    CommentId = 1,
                    EditContent = "tested content edited",
                }));

        [Fact]
        public void DeleteCommentAsyncShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Delete)
                    .WithLocation("/api/comments/delete")
                    .WithJsonBody(new
                    {
                        commentId = 1,
                    }))
                .To<CommentsApiController>(c => c.DeleteCommentAsync(new DeleteCommentRequestModel
                {
                    CommentId = 1,
                }));
    }
}
