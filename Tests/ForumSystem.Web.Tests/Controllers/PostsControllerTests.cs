namespace ForumSystem.Web.Tests.Controllers
{
    using System.Linq;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Areas.Administration.Controllers;
    using ForumSystem.Web.Controllers;
    using ForumSystem.Web.ViewModels.Posts;

    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

    using static ForumSystem.Common.GlobalConstants;
    using static ForumSystem.Web.Tests.Data.CategoiresTestData;
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
        [InlineData("Test", "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest", 1)]
        public void PostCreateShouldBeForAuthorizeUserAndShouldRedirectCorrectlyWithView(
            string title,
            string content,
            int categoryId)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(categoryId)))
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
                    .To<PostsController>(c => c.ById(1, title)));

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
        [InlineData(1, 1, false, "TestTitle1", "TestContent1")]
        public void GetEditShouldBeOnlyForAuthorizeUsersAndShouldReturnCorrectResult(
            int postId,
            int fromPage,
            bool isFromAdminPanel,
            string title,
            string content)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(postId)))
                .Calling(c => c
                    .Edit(postId, fromPage, isFromAdminPanel))
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
        [InlineData(1, 1, false, true, "TestTitle1", "TestContent1")]
        public void GetEditShouldReturnCorrectResultEvenIfUserIsNotTheOwnerButItIsTheAdministrator(
            int postId,
            int fromPage,
            bool isFromAdminPanel,
            bool isDiffOwner,
            string title,
            string content)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser(new string[] { AdministratorRoleName })
                    .WithData(GetPosts(postId, isDiffOwner)))
                .Calling(c => c
                    .Edit(postId, fromPage, isFromAdminPanel))
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
        [InlineData(1, 1, false)]
        public void GetEditShouldBeOnlyForAuthorizeUsersAndShouldReturnNotFountIfDoesntExists(
           int invalidPostId,
           int fromPage,
           bool isFromPanel)
           => MyController<PostsController>
               .Instance()
               .Calling(c => c
                   .Edit(invalidPostId, fromPage, isFromPanel))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests())
               .TempData(data => data
                    .ContainingEntryWithKey(ErrorTitleKey)
                    .ContainingEntryWithValue(ErrorNotFoundTitle)
                    .ContainingEntryWithKey(ErrorMessageKey)
                    .ContainingEntryWithValue(ErrorNotFoundMessage))
               .AndAlso()
               .ShouldReturn()
               .Redirect(red => red
                    .To<HomeController>(c => c.Error()));

        [Theory]
        [InlineData(1, 1, false, true)]
        public void GetEditShouldBeOnlyForAuthorizeUsersAndThoesWhoOwnsThePostAndShouldReturnUnautorizeIfUserNotOwner(
           int postId,
           int fromPage,
           bool isFromAdminPanel,
           bool isDiffOwner)
           => MyController<PostsController>
               .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(postId, isDiffOwner)))
               .Calling(c => c
                   .Edit(postId, fromPage, isFromAdminPanel))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .Unauthorized();

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
                    .To<PostsController>(c => c.ById(postId, editTitle)));

        [Theory]
        [InlineData(1, true, "EditTitle", "EditContentEditContentEditContentEditContentEditContent", 1)]
        public void PostEditShouldEditThePostEvenIfUserIsNotTheOwnerButItIsTheAdministrator(
           int postId,
           bool isDiffOwner,
           string editTitle,
           string editContent,
           int categoryId)
           => MyController<PostsController>
               .Instance(instance => instance
                   .WithUser(new string[] { AdministratorRoleName })
                   .WithData(GetPosts(postId, isDiffOwner)))
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
                   .To<PostsController>(c => c.ById(postId, editTitle)));

        [Theory]
        [InlineData(1, "EditTitle", "EditContentEditContentEditContentEditContentEditContent", 1)]
        public void PostEditShouldBeOnlyForAuthorizeUsersAndShouldReturnBadRequestIfDoesntExists(
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
               .BadRequest();

        [Theory]
        [InlineData(1, true, "EditTitle", "EditContentEditContentEditContentEditContentEditContent", 1)]
        public void PostEditShouldReturnUnauthorizeIfUserIsNotTheOwner(
           int postId,
           bool isDiffOwner,
           string editTitle,
           string editContent,
           int categoryId)
           => MyController<PostsController>
               .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(postId, isDiffOwner)))
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
               .Unauthorized();

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
               .InvalidModelState()
               .AndAlso()
               .ShouldReturn()
               .View(view => view
                    .WithModelOfType<int>()
                    .Passing(id => id.ShouldBe(postId)));

        [Theory]
        [InlineData(1, false, "TestTitle1", "TestContent1")]
        public void GetDeleteShouldReturnCorrectViewIfIdIsCorrect(
            int postId,
            bool isFromAdminPanel,
            string title,
            string content)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(postId)))
                .Calling(c => c.Delete(postId, isFromAdminPanel))
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
        [InlineData(1, true, false, "TestTitle1", "TestContent1")]
        public void GetDeleteShouldReturnCorrectViewIfUserIsNotTheOwnerButIsTheAdministrator(
            int postId,
            bool isDiffOwner,
            bool isFromAdminPanel,
            string title,
            string content)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser(new string[] { AdministratorRoleName })
                    .WithData(GetPosts(postId, isDiffOwner)))
                .Calling(c => c.Delete(postId, isFromAdminPanel))
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
        [InlineData(1, false)]
        public void GetDeleteShouldReturnNotFoundIfPostDoesntExits(
            int postId,
            bool isFromAdminPanel)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser())
                .Calling(c => c.Delete(postId, isFromAdminPanel))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .TempData(data => data
                    .ContainingEntryWithKey(ErrorTitleKey)
                    .ContainingEntryWithValue(ErrorNotFoundTitle)
                    .ContainingEntryWithKey(ErrorMessageKey)
                    .ContainingEntryWithValue(ErrorNotFoundMessage))
               .AndAlso()
               .ShouldReturn()
               .Redirect(red => red
                    .To<HomeController>(c => c.Error()));

        [Theory]
        [InlineData(1, true, false)]
        public void GetDeleteShouldReturnUnauthorizeIfLogInUserIsNotTheOwner(
           int postId,
           bool isDiffOwner,
           bool isFromAdminPanel)
           => MyController<PostsController>
               .Instance(instance => instance
                   .WithUser()
                   .WithData(GetPosts(postId, isDiffOwner)))
               .Calling(c => c.Delete(postId, isFromAdminPanel))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .Unauthorized();

        [Theory]
        [InlineData(1, false, 1, null)]
        public void PostDeleteShouldRedirectIfSuccessfullyDeletesCategory(
            int postId,
            bool isFromAdminPanel,
            int page,
            string searchTerm)
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
                    .To<PostsController>(c => c.All(page, searchTerm)));

        [Theory]
        [InlineData(1, true, false, 1, null)]
        public void PostDeleteShouldDeleteEvenIfUserIsNotTheOwnerButIsTheAdministrator(
         int postId,
         bool isDiffOwner,
         bool isFromAdminPanel,
         int page,
         string searchTerm)
         => MyController<PostsController>
             .Instance(instance => instance
                 .WithUser(new string[] { AdministratorRoleName })
                 .WithData(GetPosts(postId, isDiffOwner)))
             .Calling(c => c.DeleteConfirmed(postId, isFromAdminPanel))
             .ShouldHave()
             .ActionAttributes(attrs => attrs
                 .RestrictingForHttpMethod(HttpMethod.Post)
                 .RestrictingForAuthorizedRequests())
             .AndAlso()
             .ShouldReturn()
             .Redirect(redirect => redirect
                 .To<PostsController>(c => c.All(page, searchTerm)));

        [Theory]
        [InlineData(1, true, 1, null)]
        [InlineData(10, true, 1, null)]
        [InlineData(20, true, 1, null)]
        public void PostDeleteShouldRedirectToAdminPanelIfSuccessfullyDeletesCategoryFromAdminPanelFirstPage(
            int postId,
            bool isFromAdminPanel,
            int page,
            string searchTerm)
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
                    .To<PostsAdminController>(c => c.Index(page, searchTerm)));

        [Theory]
        [InlineData(1, false, 2)]
        public void PostDeleteShouldReturnBadRequestIfPostDoesntExists(
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
                .BadRequest();

        [Theory]
        [InlineData(1, true, false)]
        public void PostDeleteShouldReturnUnauthorizeIfPostDoesntExists(
            int postId,
            bool isDiffOwner,
            bool isFromAdminPanel)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(postId, isDiffOwner)))
                .Calling(c => c.DeleteConfirmed(postId, isFromAdminPanel))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Unauthorized();
    }
}
