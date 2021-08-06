namespace ForumSystem.Web.Tests.Routes.APIs
{
    using ForumSystem.Web.Controllers.APIs;
    using ForumSystem.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Http;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class UsersApiControllerTests
    {
        [Fact]
        public void EditUsernameShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/api/users/username")
                    .WithJsonBody(new
                    {
                        username = "Username-test",
                    }))
                .To<UsersApiController>(c => c.EditUsername(new EditUsernameInputModel
                {
                    Username = "Username-test",
                }));

        [Fact]
        public void UploadImageShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/api/users/image")
                    .WithJsonBody(new
                    {
                        image = With.Any<IFormFile>(),
                    }))
                .To<UsersApiController>(c => c.UploadImage(With.Any<IFormFile>()));
    }
}
