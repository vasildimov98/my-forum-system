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
        //[InlineData(10, 5, 2, 2, 2)]
        //[InlineData(5, 5, 0, 1, 1)]
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
    }
}
