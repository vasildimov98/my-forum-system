namespace ForumSystem.Web.Tests.Controllers.APIs
{
    using ForumSystem.Data.Models;
    using ForumSystem.Web.Controllers.APIs;
    using ForumSystem.Web.ViewModels.Comments;

    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using System.Linq;
    using Xunit;

    using static ForumSystem.Web.Tests.Data.CommentsTestData;

    public class CommentsApiControllerTests
    {
        [Fact]
        public void GetCommentsContentShouldBeAuthorizeAndItShouldReturnCorrectResult()
            => MyController<CommentsApiController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetComments(2)))
                .Calling(c => c.GetCommentContent(2))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Ok(ok => ok
                    .WithModelOfType<string>()
                    .Passing(text => text.ShouldBe("TestContent2")));

        [Theory]
        [InlineData(1, "<p>EditedContent<p>")]
        [InlineData(2, "<script>EditedContent</script>")]
        public void PutCommentsShouldBeAuthorizeAndItShouldReturnCorrectResultWithEditedComment(
            int commentId,
            string editContent)
            => MyController<CommentsApiController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetComments(2)))
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
                   .WithData(GetComments(2)))
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
        [InlineData(1)]
        [InlineData(2)]
        public void DeleteCommentsShouldBeAuthorizeAndItShouldReturnCorrectResult(
           int commentId)
           => MyController<CommentsApiController>
               .Instance(instance => instance
                   .WithUser()
                   .WithData(GetComments(2)))
               .Calling(c => c.DeleteCommentAsync(new DeleteCommentRequestModel
               {
                   CommentId = commentId,
               }))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests()
                   .RestrictingForHttpMethod(HttpMethod.Delete))
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
    }
}
