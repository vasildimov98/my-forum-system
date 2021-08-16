namespace ForumSystem.Web.Tests.Controllers.Admin
{
    using System.Linq;

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
        [InlineData(15, 5, 1, null, 3, 1)]
        [InlineData(10, 5, 2, null, 2, 2)]
        [InlineData(5, 5, 0, null, 1, 1)]
        public void GetIndexShouldBeRestrictedOnlyForAdministrationAndReturnCorrectResult(
            int totalPosts,
            int cateogryPerPage,
            int currentPage,
            string searchTerm,
            int totalPages,
            int expectedCurrPage)
            => MyController<PostsAdminController>
                .Instance(instance => instance
                        .WithUser()
                        .WithData(GetPosts(totalPosts)))
                .Calling(c => c.Index(currentPage, searchTerm))
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
    }
}
