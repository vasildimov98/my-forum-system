namespace ForumSystem.Web.Tests.Controllers
{
    using System.Linq;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Controllers;
    using ForumSystem.Web.ViewModels.Posts;

    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

    using static ForumSystem.Web.Tests.Data.PostsTestData;

    public class PostsControllerTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("selected")]
        public void GetCreateShouldReturnCorrectViewModel(string selected)
            => MyController<PostsController>
                .Instance()
                .Calling(c => c.Create(selected))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<PostInputModel>()
                    .Passing(postInputModel =>
                    {
                        postInputModel.Categories.ShouldNotBeNull();
                        postInputModel.Selected = selected;
                    }));

        [Theory]
        [InlineData("Test", "TestTestTestTestTestTest", 1)]
        public void PostCreateShouldBeForAuthorizeUserAndShouldRedirectCorrectlyWithView(
            string title,
            string content,
            int categoryId)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser())
                .Calling(c => c.Create(new PostInputModel
                {
                    Title = title,
                    Content = content,
                    CategoryId = categoryId,
                }))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Post>(posts => posts
                        .Any(p =>
                             p.Title == title &&
                             p.CategoryId == categoryId &&
                             p.Content == content)))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<PostsController>(c => c.ById(With.Any<int>())));

        [Theory]
        [InlineData("Test", "Test", 1)]
        [InlineData(
            @"Title 
                      with new line",
            "TestTestTestTestTestTest",
            1)]
        public void PostCreateShouldReturnTheSameViewIfModelStateIsIncorrect(
            string title,
            string invalidContent,
            int categoryId)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser())
                .Calling(c => c.Create(new PostInputModel
                {
                    Title = title,
                    Content = invalidContent,
                    CategoryId = categoryId,
                }))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .Data(data => data
                    .WithSet<Post>(posts => !posts
                        .Any(p =>
                             p.Title == title &&
                             p.CategoryId == categoryId &&
                             p.Content == invalidContent)))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<PostInputModel>()
                    .Passing(model =>
                    {
                        model.Title.ShouldBe(title);
                        model.Content.ShouldBe(invalidContent);
                        model.CategoryId.ShouldBe(categoryId);
                    }));

        [Theory]
        [InlineData(1, "TestTitle1", "TestContent1")]
        public void GetEditShouldBeOnlyForAuthorizeUsersAndShouldReturnCorrectResult(
            int postId,
            string title,
            string content)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(postId)))
                .Calling(c => c
                    .Edit(postId))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<PostEditModel>()
                    .Passing(editModel =>
                    {
                        editModel.Title.ShouldBe(title);
                        editModel.Content.ShouldBe(content);
                    }));

        [Theory]
        [InlineData(1)]
        public void GetEditShouldBeOnlyForAuthorizeUsersAndShouldReturnNotFountIfDoesntExists(
           int invalidPostId)
           => MyController<PostsController>
               .Instance()
               .Calling(c => c
                   .Edit(invalidPostId))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .NotFound();

        [Theory]
        [InlineData(1, "EditTitle", "EditContentEditContentEditContentEditContentEditContent", 1)]
        public void PostEditShouldBeOnlyForAuthorizeUsersAndShouldReturnCorrectResult(
            int postId,
            string editTitle,
            string editContent,
            int categoryId)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(postId)))
                .Calling(c => c
                    .Edit(new PostEditModel
                    {
                        Id = postId,
                        Title = editTitle,
                        Content = editContent,
                        CategoryId = categoryId,
                    }))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .Data(data => data
                    .WithSet<Post>(post => post
                        .Any(p => p.Id == postId &&
                                  p.Title == editTitle &&
                                  p.Content == editContent &&
                                  p.CategoryId == categoryId)))
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ForumSystem.Web.Controllers.PostsController>(c => c.ById(postId)));

        [Theory]
        [InlineData(1, "EditTitle", "EditContentEditContentEditContentEditContentEditContent", 1)]
        public void PostEditShouldBeOnlyForAuthorizeUsersAndShouldReturnNotFountIfDoesntExists(
           int postId,
           string editTitle,
           string editContent,
           int categoryId)
           => MyController<PostsController>
               .Instance()
               .Calling(c => c
                    .Edit(new PostEditModel
                    {
                        Id = postId,
                        Title = editTitle,
                        Content = editContent,
                        CategoryId = categoryId,
                    }))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForHttpMethod(HttpMethod.Post)
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .NotFound();

        [Theory]
        [InlineData(1, "editName", "EditDesctiption", 1)]
        public void PostEditShouldReturnToSameViewIfContentTooShortNotValid(
           int postId,
           string editTitle,
           string shortContent,
           int categoryId)
           => MyController<PostsController>
               .Instance()
               .Calling(c => c
                    .Edit(new PostEditModel
                    {
                        Id = postId,
                        Title = editTitle,
                        Content = shortContent,
                        CategoryId = categoryId,
                    }))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForHttpMethod(HttpMethod.Post)
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View(view => view
                    .WithModelOfType<int>()
                    .Passing(id => id.ShouldBe(postId)));

        [Theory]
        [InlineData(1, "TestTitle1", "TestContent1")]
        public void GetDeleteShouldReturnCorrectViewIfIdIsCorrect(
            int postId,
            string title,
            string content)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(postId)))
                .Calling(c => c.Delete(postId))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<PostEditModel>()
                    .Passing(model =>
                    {
                        model.Title.ShouldBe(title);
                        model.Content.ShouldBe(content);
                    }));

        [Theory]
        [InlineData(null)]
        public void GetDeleteShouldReturnNotFoundIfIdIsNull(
            int? postId)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(1)))
                .Calling(c => c.Delete(postId))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .NotFound();

        [Theory]
        [InlineData(1)]
        public void GetDeleteShouldReturnNotFoundIfCategoryDoesntExits(
            int postId)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser())
                .Calling(c => c.Delete(postId))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .NotFound();

        [Theory]
        [InlineData(1, false, 1)]
        public void PostDeleteShouldRedirectIfSuccessfullyDeletesCategory(
            int postId,
            bool isFromAdminPanel,
            int page)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(postId)))
                .Calling(c => c.DeleteConfirmed(postId, isFromAdminPanel))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ForumSystem.Web.Controllers.PostsController>(c => c.All(page)));

        [Theory]
        [InlineData(1, false, 2)]
        public void PostDeleteShouldReturnNotFoundIfCategoryDoesntExists(
            int postId,
            bool isFromAdminPanel,
            int invalidPostId)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(postId)))
                .Calling(c => c.DeleteConfirmed(invalidPostId, isFromAdminPanel))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .NotFound();
    }
}
