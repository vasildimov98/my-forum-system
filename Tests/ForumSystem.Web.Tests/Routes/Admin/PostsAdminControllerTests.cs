namespace ForumSystem.Web.Tests.Routes.Admin
{
    using ForumSystem.Web.Areas.Administration.Controllers;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class PostsAdminControllerTests
    {
        [Fact]
        public void GetIndexShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("Administration/Posts/Index")
                .To<PostsController>(c => c.Index(With.Value<int>(1)));

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
