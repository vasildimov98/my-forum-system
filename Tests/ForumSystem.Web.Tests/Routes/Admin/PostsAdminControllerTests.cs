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
                .ShouldMap("Administration/PostsAdmin/Index")
                .To<PostsAdminController>(c => c.Index(With.Value<int>(1), With.Value<string>(null)));
    }
}
