namespace ForumSystem.Web.Tests.Controllers
{
    using System.Linq;

    using ForumSystem.Web.Controllers;
    using ForumSystem.Web.ViewModels.Chat;
    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

    using static ForumSystem.Web.Tests.Data.CategoiresTestData;

    public class CategoriesControllerTests
    {
        [Theory]
        [InlineData(10, 1, 2)]
        public void GetAllShouldReturnCorrectViewModel(
            int categoriesCount,
            int page,
            int totalPages)
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithData(GetCategories(12)))
                .Calling(c => c.All(page))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CategoryViewModelList>()
                    .Passing(categoriesViewModel =>
                    {
                        categoriesViewModel.Categories.Count().ShouldBe(categoriesCount);
                        categoriesViewModel.PaginationModel.CurrentPage.ShouldBe(page);
                        categoriesViewModel.PaginationModel.TotalPages.ShouldBe(totalPages);
                    }));
    }
}
