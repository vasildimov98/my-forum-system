namespace ForumSystem.Web.Tests.Controllers.APIs
{
    using System.Linq;

    using ForumSystem.Data.Models;
    using ForumSystem.Data.Models.Enums;
    using ForumSystem.Web.Controllers.APIs;
    using ForumSystem.Web.ViewModels.Votes;

    using MyTested.AspNetCore.Mvc;
    using Shouldly;

    using Xunit;

    using static ForumSystem.Web.Tests.Data.CommentsTestData;
    using static ForumSystem.Web.Tests.Data.PostsTestData;

    public class VotesApiControllerTests
    {
        [Theory]
        [InlineData(1, true, 1)]
        [InlineData(2, false, -1)]
        public void PostPostVoteShouldBeAuthorizeAndRestrictedToPostAndReturnCorrectResult(
            int postId,
            bool isUpVote,
            int expected)
            => MyController<VotesApiController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(2)))
                .Calling(c => c.PostPostVote(new VoteOnPostInpuModel
                {
                    Id = postId,
                    IsUpVote = isUpVote,
                }))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .Data(data => data
                    .WithSet<Vote>(votes => votes
                        .Any(v => v.PostId == postId &&
                             v.Type == (VoteType)expected)))
                .AndAlso()
                .ShouldReturn()
                .Object(obj => obj
                    .WithModelOfType<VoteResponseModel>()
                    .Passing(resp =>
                    {
                        resp.VotesCount.ShouldBe(expected);
                    }));

        [Theory]
        [InlineData(1, true, 1)]
        [InlineData(2, false, -1)]
        public void PostCommentVoteShouldBeAuthorizeAndRestrictedToPostAndReturnCorrectResult(
            int commentId,
            bool isUpVote,
            int expected)
            => MyController<VotesApiController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetComments(2)))
                .Calling(c => c.PostCommentVote(new VoteOnCommentInputModel
                {
                    Id = commentId,
                    IsUpVote = isUpVote,
                }))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .Data(data => data
                    .WithSet<Comment>(comments => comments
                        .Any(c => c.Id == commentId &&
                             c.CommentVotes.Any(v => v.Type == (VoteType)expected))))
                .AndAlso()
                .ShouldReturn()
                .Object(obj => obj
                    .WithModelOfType<VoteResponseModel>()
                    .Passing(resp =>
                    {
                        resp.VotesCount.ShouldBe(expected);
                    }));
    }
}
