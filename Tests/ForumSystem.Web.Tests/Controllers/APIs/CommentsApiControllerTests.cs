namespace ForumSystem.Web.Tests.Controllers.APIs
{
    using System.Linq;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Controllers.APIs;
    using ForumSystem.Web.ViewModels.Comments;

    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

    using static ForumSystem.Web.Tests.Data.CommentsTestData;

    public class CommentsApiControllerTests
    {
        [Theory]
        [InlineData(2, "TestContent2")]
        public void GetCommentsContentShouldBeAuthorizeAndItShouldReturnCorrectResult(
            int commentId,
            string content)
            => MyController<CommentsApiController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetComments(commentId)))
                .Calling(c => c.GetCommentContent(commentId))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Ok(ok => ok
                    .WithModelOfType<string>()
                    .Passing(text => text.ShouldBe(content)));

        [Theory]
        [InlineData(1, 2)]
        public void GetCommentsContentShouldBeAuthorizeAndReturnNotFoundIfThereIsNoSuchComment(
            int commentId,
            int invalidCommentId)
            => MyController<CommentsApiController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetComments(commentId)))
                .Calling(c => c.GetCommentContent(invalidCommentId))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .NotFound();

        [Theory]
        [InlineData(1, "<p>EditedContent<p>")]
        [InlineData(2, "<script>EditedContent</script>")]
        public void PutCommentsShouldBeAuthorizeAndItShouldReturnCorrectResultWithEditedComment(
            int commentId,
            string editContent)
            => MyController<CommentsApiController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetComments(commentId)))
                .Calling(c => c.PutCommentAsync(new EditCommentJsonModel
                {
                    CommentId = commentId,
                    EditContent = editContent,
                }))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Put))
                .Data(data => data
                    .WithSet<Comment>(comment => comment
                        .Any(c => c.Id == commentId &&
                            c.Content == editContent)))
                .AndAlso()
                .ShouldReturn()
                .Ok(ok => ok
                    .WithModelOfType<EditCommentViewModel>()
                    .Passing(editCommentViewModel =>
                    {
                        editCommentViewModel.Content.ShouldBe(editContent);
                    }));

        [Theory]
        [InlineData(1, "<script>alert(EditedContent)</script>")]
        [InlineData(2, "<script>EditedContent</script>")]
        public void PutCommentsShouldBeAuthorizeAndItShouldReturnCorectSanitizeResult(
           int commentId,
           string editContent)
           => MyController<CommentsApiController>
               .Instance(instance => instance
                   .WithUser()
                   .WithData(GetComments(commentId)))
               .Calling(c => c.PutCommentAsync(new EditCommentJsonModel
               {
                   CommentId = commentId,
                   EditContent = editContent,
               }))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests()
                   .RestrictingForHttpMethod(HttpMethod.Put))
               .Data(data => data
                    .WithSet<Comment>(comment => comment
                        .Any(c => c.Id == commentId &&
                            c.Content == editContent)))
               .AndAlso()
               .ShouldReturn()
               .Ok(ok => ok
                   .WithModelOfType<EditCommentViewModel>()
                   .Passing(editCommentViewModel =>
                   {
                       editCommentViewModel.SanitizeContent.ShouldBeEmpty();
                   }));

        [Theory]
        [InlineData(0, 1, "<script>alert(EditedContent)</script>")]
        [InlineData(2, 3, "<script>EditedContent</script>")]
        public void PutCommentsShouldBeAuthorizeAndItShouldReturnNotFoundIfThereIsNoSuchComment(
          int commentId,
          int invalidCommentId,
          string editContent)
          => MyController<CommentsApiController>
              .Instance(instance => instance
                  .WithUser()
                  .WithData(GetComments(commentId)))
              .Calling(c => c.PutCommentAsync(new EditCommentJsonModel
              {
                  CommentId = invalidCommentId,
                  EditContent = editContent,
              }))
              .ShouldHave()
              .ActionAttributes(attrs => attrs
                  .RestrictingForAuthorizedRequests()
                  .RestrictingForHttpMethod(HttpMethod.Put))
              .Data(data => data
                   .WithSet<Comment>(comment => !comment
                       .Any(c => c.Id == commentId &&
                           c.Content == editContent)))
              .AndAlso()
              .ShouldReturn()
              .NotFound();

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void DeleteCommentsShouldBeAuthorizeAndItShouldReturnCorrectResult(
           int commentId)
           => MyController<CommentsApiController>
               .Instance(instance => instance
                   .WithUser()
                   .WithData(GetComments(2)))
               .Calling(c => c.DeleteCommentAsync(new DeleteCommentJsonModel
               {
                   CommentId = commentId,
               }))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForHttpMethod(HttpMethod.Delete)
                   .RestrictingForAuthorizedRequests())
               .Data(data => data
                    .WithSet<Comment>(comment => comment
                        .Any(c => !(c.Id == commentId))))
               .AndAlso()
               .ShouldReturn()
               .Ok(ok => ok
                   .WithModelOfType<string>()
                   .Passing(text =>
                   {
                       text.ShouldBe("Delete is successfull");
                   }));

        [Theory]
        [InlineData(1, 2, "TestContent1")]
        [InlineData(2, 3, "TestContent2")]
        public void DeleteCommentsShouldBeAuthorizeAndItShouldReturnNotFoundIfThereIsNoSuchComment(
          int commentId,
          int invalidCommentId,
          string content)
          => MyController<CommentsApiController>
              .Instance(instance => instance
                  .WithUser()
                  .WithData(GetComments(commentId)))
              .Calling(c => c.DeleteCommentAsync(new DeleteCommentJsonModel
              {
                  CommentId = invalidCommentId,
              }))
              .ShouldHave()
              .ActionAttributes(attrs => attrs
                  .RestrictingForHttpMethod(HttpMethod.Delete)
                  .RestrictingForAuthorizedRequests())
              .Data(data => data
                   .WithSet<Comment>(comment => comment
                       .Any(c => c.Id == commentId &&
                           c.Content == content)))
              .AndAlso()
              .ShouldReturn()
              .NotFound();
    }
}
