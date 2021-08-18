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
        [InlineData(5, 5, 1, null, 1, 1)]
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

        [Theory]
        [InlineData(10, -1, null, 1)]
        [InlineData(12, 0, null, 1)]
        [InlineData(20, -2, null, 1)]
        [InlineData(10, -123, "4", 1)]
        [InlineData(12, 0, "tes", 1)]
        [InlineData(40, -5, "3", 1)]
        public void GetIndexShouldRedirectToTheFirstPageOfSameActionIfGivenIdIsLessOrEqualToZero(
            int total,
            int page,
            string searchTerm,
            int expectedPage)
            => MyController<PostsAdminController>
                .Instance(instance => instance
                    .WithData(GetPosts(total)))
                .Calling(c => c.Index(page, searchTerm))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .TempData(data => data
                    .ContainingEntryWithKey(InvalidMessageKey)
                    .ContainingEntryWithValue(InvalidPageRequest))
                .AndAlso()
                .ShouldReturn()
                .Redirect(red => red
                    .To<PostsAdminController>(c => c.Index(expectedPage, searchTerm)));

        [Theory]
        [InlineData(10, 123, null, 2)]
        [InlineData(12, 1000, null, 3)]
        [InlineData(40, 10, null, 8)]
        [InlineData(10, 123, "4", 1)]
        [InlineData(12, 1000, "tes", 3)]
        [InlineData(40, 5, "3", 3)]
        public void GetIndexShouldRedirectToTheLastPageOfSameActionIfGivenIdIsMoreThanTotalPages(
            int total,
            int page,
            string searchTerm,
            int expectedPage)
            => MyController<PostsAdminController>
                .Instance(instance => instance
                    .WithData(GetPosts(total)))
                .Calling(c => c.Index(page, searchTerm))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .TempData(data => data
                    .ContainingEntryWithKey(InvalidMessageKey)
                    .ContainingEntryWithValue(InvalidPageRequest))
                .AndAlso()
                .ShouldReturn()
                .Redirect(red => red
                    .To<PostsAdminController>(c => c.Index(expectedPage, searchTerm)));
    }
}
