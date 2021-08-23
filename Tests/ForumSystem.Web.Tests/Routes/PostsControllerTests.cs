namespace ForumSystem.Web.Tests.Routes
{
    using ForumSystem.Web.Controllers;
    using ForumSystem.Web.ViewModels.Posts;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class PostsControllerTests
    {
        [Fact]
        public void GetIndexShouldBeRoutedToPostsAll()
            => MyRouting
                .Configuration()
                .ShouldMap("/")
                .To<PostsController>(c => c.All(With.No<int>(), With.No<string>()));

        [Fact]
        public void GetAllShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Posts/All/1")
                .To<PostsController>(c => c.All(With.Value<int>(1), With.Value<string>(null)));

        [Fact]
        public void GetCreateShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Posts/Create")
                .To<PostsController>(c => c.Create(With.Value<string>("selected")));

        [Fact]
        public void PostCreateShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Posts/Create"))
                .To<PostsController>(c => c.Create(With.Any<PostInputModel>()));

        [Fact]
        public void GetByIdShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Post/3/test_test")
                .To<PostsController>(c => c.ById(With.Value<int>(3), With.Value<string>("test_test")));

        [Fact]
        public void GetEditShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("Post/Edit/1")
                .To<PostsController>(c => c.Edit(With.Value<int>(1), With.Value<int>(1), With.Value<bool>(false)));

        [Fact]
        public void GetDeleteShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("Post/Delete/1")
                .To<PostsController>(c => c.Delete(With.Value<int>(1), With.Value<bool>(false)));

        [Theory]
        [InlineData(1, true, "Post/Delete/1?isFromAdminPanel=True")]
        public void GetDeleteCallFromAdminPanelShouldBeRoutedCorrectly(
            int postId,
            bool isFromAdminPanel,
            string location)
            => MyRouting
                .Configuration()
                .ShouldMap(location)
                .To<PostsController>(c => c.Delete(postId, isFromAdminPanel));

        [Theory]
        [InlineData(2, "Post/Delete/2")]
        public void PostDeleteShouldBeRoutedCorrectly(
            int id,
            string location)
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation(location))
                .To<PostsController>(c => c.DeleteConfirmed(id, With.Value<bool>(false)));

        [Theory]
        [InlineData(2, true, "Post/Delete/2?isFromAdminPanel=True")]
        public void PostDeleteShouldBeRoutedCorrectlyIfFromAdminPanel(
           int id,
           bool isFromAdminPanel,
           string location)
           => MyRouting
               .Configuration()
               .ShouldMap(request => request
                   .WithMethod(HttpMethod.Post)
                   .WithLocation(location))
               .To<PostsController>(c => c.DeleteConfirmed(id, isFromAdminPanel));
    }
}
