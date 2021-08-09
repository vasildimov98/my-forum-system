namespace ForumSystem.Web.Tests.Controllers
{
    using System.Linq;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Controllers;
    using ForumSystem.Web.ViewModels.Posts;

    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

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
                    CategoryId = categoryId,
                    Content = content,
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
    }
}
