namespace ForumSystem.Web.Tests.Routes
{
    using ForumSystem.Web.Controllers;
    using ForumSystem.Web.ViewModels.Categories;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class CategoriesControllerTests
    {
        [Fact]
        public void GetAllShouldRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Categories/All")
                .To<CategoriesController>(c => c.All(With.Value<int>(1), With.Value<string>(null)));

        [Theory]
        [InlineData("TestUser", 1, null, "/Categories/ByOwner/TestUser/1")]
        [InlineData("TestUser2", 2, null, "/Categories/ByOwner/TestUser2/2")]
        public void GetByOwnerShouldRoutedCorrectly(
            string username,
            int page,
            string searchTerm,
            string location)
            => MyRouting
                .Configuration()
                .ShouldMap(location)
                .To<CategoriesController>(c => c.ByOwner(username, page, searchTerm));

        [Fact]
        public void GetByNameShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("Category/test-test/1")
                .To<CategoriesController>(c => c.ByName("test-test", 1, null));

        [Fact]
        public void GetCreateShouldBeRoutedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("Category/Create")
               .To<CategoriesController>(c => c
                            .Create());

        [Fact]
        public void PostCreateShouldBeRoutedCorrectly()
         => MyRouting
             .Configuration()
             .ShouldMap("Category/Create")
             .To<CategoriesController>(c => c
                            .Create(With.Any<CategoryInputModel>()));

        [Fact]
        public void GetEditShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("Category/Edit/1")
                .To<CategoriesController>(c => c.Edit(With.Value<int>(1), With.Value<int>(1), With.Value<bool>(false)));

        [Fact]
        public void PostEditShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("Category/Edit/1"))
                .To<CategoriesController>(c => c.Edit(With.Value<int>(1), With.Value<int>(1), With.Value<bool>(false)));

        [Fact]
        public void GetDeleteShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("Category/Delete/1")
                .To<CategoriesController>(c => c.Delete(With.Value<int>(1), With.Value<bool>(false)));

        [Theory]
        [InlineData(2, "Category/Delete/2")]
        public void PostDeleteShouldBeRoutedCorrectly(
            int id,
            string location)
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation(location))
                .To<CategoriesController>(c => c.DeleteConfirmed(id, With.Value<bool>(false)));

        [Theory]
        [InlineData(2, true, "Category/Delete/2?isFromAdminPanel=True")]
        public void PostDeleteShouldBeRoutedCorrectlyIfFromAdminPanel(
           int id,
           bool isFromAdminPanel,
           string location)
           => MyRouting
               .Configuration()
               .ShouldMap(request => request
                   .WithMethod(HttpMethod.Post)
                   .WithLocation(location))
               .To<CategoriesController>(c => c.DeleteConfirmed(id, isFromAdminPanel));
    }
}
