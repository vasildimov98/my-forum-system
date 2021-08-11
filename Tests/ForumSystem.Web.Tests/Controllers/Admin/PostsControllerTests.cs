namespace ForumSystem.Web.Tests.Controllers.Admin
{
    using System.Linq;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Areas.Administration.Controllers;
    using ForumSystem.Web.ViewModels.Administration.Posts;

    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

    using static ForumSystem.Common.GlobalConstants;
    using static ForumSystem.Web.Tests.Data.PostsTestData;

    public class PostsControllerTests
    {
        [Theory]
        [InlineData(15, 5, 1, 3, 1)]
        [InlineData(10, 5, 2, 2, 2)]
        [InlineData(5, 5, 0, 1, 1)]
        public void GetIndexShouldBeRestrictedOnlyForAdministrationAndReturnCorrectResult(
            int totalPosts,
            int cateogryPerPage,
            int currentPage,
            int totalPages,
            int expectedCurrPage)
            => MyController<PostsController>
                .Instance(instance => instance
                        .WithUser()
                        .WithData(GetPosts(totalPosts)))
                .Calling(c => c.Index(currentPage))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests(withAllowedRoles: AdministratorRoleName))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<PostCrudModelList>()
                    .Passing(postCrudModel =>
                    {
                        postCrudModel.Posts
                            .Count()
                            .ShouldBe(cateogryPerPage);
                        postCrudModel.PaginationModel.CurrentPage
                            .ShouldBe(expectedCurrPage);
                        postCrudModel.PaginationModel.TotalPages
                            .ShouldBe(totalPages);
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
        [InlineData(1, 1)]
        public void PostDeleteShouldRedirectIfSuccessfullyDeletesCategory(
            int postId,
            int page)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(postId)))
                .Calling(c => c.DeleteConfirmed(postId))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ForumSystem.Web.Controllers.PostsController>(c => c.All(page)));

        [Theory]
        [InlineData(1, 2)]
        public void PostDeleteShouldReturnNotFoundIfCategoryDoesntExists(
            int postId,
            int invalidPostId)
            => MyController<PostsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetPosts(postId)))
                .Calling(c => c.DeleteConfirmed(invalidPostId))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .NotFound();
    }
}
