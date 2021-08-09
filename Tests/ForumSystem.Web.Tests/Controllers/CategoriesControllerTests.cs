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
        [Fact]
        public void GetAllShouldReturnCorrectViewModel()
            => MyController<CategoriesController>
                .Instance(instance => instance
                    .WithData(GetCategories(20)))
                .Calling(c => c.All(1))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CategoryViewModelList>()
                    .Passing(categoriesViewModel =>
                    {
                        categoriesViewModel.Categories.Count().ShouldBe(10);
                        categoriesViewModel.PaginationModel.CurrentPage.ShouldBe(1);
                        categoriesViewModel.PaginationModel.PagesCount.ShouldBe(2);
                    }));
    }
}
