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
                .To<PostsController>(c => c.All(With.No<int>()));

        [Fact]
        public void GetAllShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Posts/All/1")
                .To<PostsController>(c => c.All(With.Value<int>(1)));

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
                .ShouldMap("/Posts/ById/3")
                .To<PostsController>(c => c.ById(With.Value<int>(3)));

        [Fact]
        public void GetEditShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("Post/Edit/1")
                .To<PostsController>(c => c.Edit(With.Value<int>(1)));

        [Fact]
        public void PostEditShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("Post/Edit/1"))
                .To<PostsController>(c => c.Edit(With.Any<int>()));

        [Fact]
        public void GetDeleteShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("Post/Delete/1")
                .To<PostsController>(c => c.Delete(With.Value<int>(1)));

        [Fact]
        public void PostDeleteShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("Post/Delete/1"))
                .To<PostsController>(c => c.DeleteConfirmed(With.Value<int>(1)));
    }
}
