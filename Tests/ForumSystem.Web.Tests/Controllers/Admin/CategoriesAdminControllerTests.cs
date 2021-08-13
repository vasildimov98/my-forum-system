namespace ForumSystem.Web.Tests.Controllers.Admin
{
    using System.Linq;

    using ForumSystem.Web.Areas.Administration.Controllers;
    using ForumSystem.Web.ViewModels.Administration.Categories;

    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

    using static ForumSystem.Common.GlobalConstants;
    using static ForumSystem.Web.Tests.Data.CategoiresTestData;

    public class CategoriesAdminControllerTests
    {
        [Theory]
        [InlineData(15, 5, 1, 3, 1)]
        [InlineData(10, 5, 2, 2, 2)]
        [InlineData(5, 5, 0, 1, 1)]
        public void GetIndexShouldBeRestrictedOnlyForAdministrationAndReturnCorrectResult(
            int totalCategories,
            int cateogryPerPage,
            int currentPage,
            int totalPages,
            int expectedCurrPage)
            => MyController<CategoriesAdminController>
                .Instance(instance => instance
                        .WithUser()
                        .WithData(GetCategories(totalCategories)))
                .Calling(c => c.Index(currentPage))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests(withAllowedRoles: AdministratorRoleName))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CategoryCrudModelList>()
                    .Passing(categoryCrudModel =>
                    {
                        categoryCrudModel.Categories
                            .Count()
                            .ShouldBe(cateogryPerPage);
                        categoryCrudModel.PaginationModel.CurrentPage
                            .ShouldBe(expectedCurrPage);
                        categoryCrudModel.PaginationModel.TotalPages
                            .ShouldBe(totalPages);
                    }));

        [Theory]
        [InlineData(10, 5, 1, 5, 2)]
        [InlineData(12, 10, 1, 5, 3)]
        [InlineData(20, 20, 2, 5, 4)]
        public void GetAllShouldReturnEverySingleCateogryEvenIfNotUnapproved(
            int total,
            int approved,
            int page,
            int expectedCategories,
            int expectedPages)
            => MyController<CategoriesAdminController>
                .Instance(instance => instance
                    .WithData(GetMixedCategories(total, approved)))
                .Calling(c => c.Index(page))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests(withAllowedRoles: AdministratorRoleName))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CategoryCrudModelList>()
                    .Passing(categoriesViewModel =>
                    {
                        categoriesViewModel.Categories.Count().ShouldBe(expectedCategories);
                        categoriesViewModel.PaginationModel.CurrentPage.ShouldBe(page);
                        categoriesViewModel.PaginationModel.TotalPages.ShouldBe(expectedPages);
                    }));

        [Theory]
        [InlineData(1, 1)]
        [InlineData(6, 2)]
        public void GetApproveCategoryShouldRedirectToIndexWithTempMessageIfCategoryIdIsWrong(
            int categoryId,
            int page)
            => MyController<CategoriesAdminController>
                .Instance()
                .Calling(c => c.Approve(categoryId, page))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests(withAllowedRoles: AdministratorRoleName))
                .TempData(temp => temp
                    .ContainingEntryWithKey(InvalidMessageKey)
                    .ContainingEntryWithValue(InvalidApprovalMessage))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<CategoriesAdminController>(c => c.Index(page)));

        [Theory]
        [InlineData(1, false, 1)]
        [InlineData(6, false, 2)]
        public void GetApproveShouldRedirectToIndexWithoutTempDataIfCategoryIsApproveSuccessfully(
            int categoryId,
            bool isApprove,
            int page)
            => MyController<CategoriesAdminController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(categoryId, isApprove)))
                .Calling(c => c.Approve(categoryId, page))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests(withAllowedRoles: AdministratorRoleName))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<CategoriesAdminController>(c => c.Index(page)));

    }
}
