namespace ForumSystem.Web.Tests.Routes.APIs
{
    using ForumSystem.Web.Controllers.APIs;
    using ForumSystem.Web.ViewModels.Votes;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class VotesApiControllerTests
    {
        [Fact]
        public void PostPostVoteShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/api/votes")
                    .WithJsonBody(new
                    {
                        id = 1,
                        isUpVote = true,
                    }))
                .To<VotesApiController>(c => c.PostPostVote(new VoteOnPostInpuModel
                {
                    Id = 1,
                    IsUpVote = true,
                }));

        [Fact]
        public void PostCommentVoteShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/api/votes/comment")
                    .WithJsonBody(new
                    {
                        id = 1,
                        isUpVote = true,
                    }))
                .To<VotesApiController>(c => c.PostCommentVote(new VoteOnCommentInputModel
                {
                    Id = 1,
                    IsUpVote = true,
                }));
    }
}
