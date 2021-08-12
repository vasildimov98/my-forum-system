namespace ForumSystem.Web.Tests.Controllers.APIs
{
    using System.Linq;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Controllers.APIs;
    using ForumSystem.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Http;
    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

    using static ForumSystem.Common.GlobalConstants;
    using static ForumSystem.Web.Tests.Data.FilesTestData;
    using static ForumSystem.Web.Tests.Data.UsersTestData;

    public class UsersApiControllerApiTests
    {
        [Theory]
        [InlineData("EditUserName1")]
        public void EditUsernameShouldBeAuthorizeAndShouldReturnCorrectResult(
            string username)
            => MyController<UsersApiController>
                .Instance(instance => instance
                    .WithData(GetUsers(1))
                    .WithUser())
                .Calling(c => c.EditUsername(new EditUsernameInputModel
                {
                    Username = username,
                }))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Put))
                .Data(data => data
                    .WithSet<ApplicationUser>(user => user
                        .Any(u => u.UserName == username)))
                .AndAlso()
                .ShouldReturn()
                .Object(obj => obj
                    .WithModelOfType<EditResponseModel>()
                    .Passing(editModel =>
                    {
                        editModel.Username.ShouldBe(username);
                        editModel.ErrorMessage.ShouldBeNull();
                    }));

        [Theory]
        [InlineData(
                    "TestUserId1",
                    "TestUserName1",
                    "This username is taken. Try another one!")]
        public void EditUsernameShouldBeAuthorizeAndShouldReturnCorrectErrorIfUsernameIsTaken(
            string userId,
            string username,
            string errorMessage)
            => MyController<UsersApiController>
                .Instance(instance => instance
                    .WithData(GetUsers(1))
                    .WithUser(userId, username))
                .Calling(c => c.EditUsername(new EditUsernameInputModel
                {
                    Username = TestUser.Username,
                }))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Put))
                .Data(data => data
                    .WithSet<ApplicationUser>(user => user
                        .Count(u => u.UserName == username)
                            .ShouldBe(1)))
                .AndAlso()
                .ShouldReturn()
                .Object(obj => obj
                    .WithModelOfType<EditResponseModel>()
                    .Passing(editModel =>
                    {
                        editModel.Username.ShouldBe(username);
                        editModel.ErrorMessage.ShouldBe(errorMessage);
                    }));

        [Theory]
        [InlineData(".jpg", "/profileImages/")]
        public void PostUploadImageShouldBeAuthorizeAndShouldReturnAndSaveCorrectData(
            string extention,
            string expected)
            => MyController<UsersApiController>
                .Instance(instance => instance
                    .WithData(GetUsers(1))
                    .WithUser())
                .Calling(c => c
                    .UploadImage(GetFile()))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .Data(data => data
                    .WithSet<ProfileImage>(profileImage => profileImage
                        .Any(img => img.Extention == extention)))
                .AndAlso()
                .ShouldReturn()
                .Ok(ok => ok
                    .WithModelOfType<string>()
                    .Passing(imagePath => imagePath.ShouldContain(expected)));

        [Theory]
        [InlineData(".jpg")]
        public void PostUploadImageShouldBeAuthorizeAndShouldReturnBadRequestIfAnythingGoesWrong(
            string extention)
            => MyController<UsersApiController>
                .Instance(instance => instance
                    .WithData(GetUsers(1))
                    .WithUser())
                .Calling(c => c
                    .UploadImage(With.Any<IFormFile>()))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .Data(data => data
                    .WithSet<ProfileImage>(profileImage => !profileImage
                        .Any(img => img.Extention == extention)))
                .AndAlso()
                .ShouldReturn()
                .BadRequest(badRequest => badRequest
                    .WithErrorMessage("File is missing or its too big! Allowed length is 10MB"));

        [Theory]
        [InlineData(".jpg")]
        public void PostUploadImageShouldBeAuthorizeAndShouldReturnBadRequestIfFileLengthIsTooLong(
           string extention)
           => MyController<UsersApiController>
               .Instance(instance => instance
                   .WithData(GetUsers(1))
                   .WithUser())
               .Calling(c => c
                   .UploadImage(GetToLongFile()))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForHttpMethod(HttpMethod.Post)
                   .RestrictingForAuthorizedRequests())
               .Data(data => data
                   .WithSet<ProfileImage>(profileImage => !profileImage
                       .Any(img => img.Extention == extention)))
               .AndAlso()
               .ShouldReturn()
               .BadRequest(badRequest => badRequest
                   .WithErrorMessage(InvalidFileImageLengthMessage));

        [Theory]
        [InlineData(".tif")]
        public void PostUploadImageShouldBeAuthorizeAndShouldReturnBadRequestIfFileExtentionIsInvalid(
           string invalidExtention)
           => MyController<UsersApiController>
               .Instance(instance => instance
                   .WithData(GetUsers(1))
                   .WithUser())
               .Calling(c => c
                   .UploadImage(GetFileWithInvalidExtention()))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForHttpMethod(HttpMethod.Post)
                   .RestrictingForAuthorizedRequests())
               .Data(data => data
                   .WithSet<ProfileImage>(profileImage => !profileImage
                       .Any(img => img.Extention == invalidExtention)))
               .AndAlso()
               .ShouldReturn()
               .BadRequest(badRequest => badRequest
                   .WithErrorMessage(InvalidFileImageExtentionMessage));
    }
}
