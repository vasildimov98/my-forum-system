namespace ForumSystem.Web.Tests.Routes
{
    using ForumSystem.Web.Controllers;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class CategoriesControllerTests
    {
        [Fact]
        public void GetAllShouldRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Categories/All")
                .To<CategoriesController>(c => c.All(With.Value<int>(1)));

        [Fact]
        public void GetByNameShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("Category/test-test/1")
                .To<CategoriesController>(c => c.ByName("test-test", 1));
    }
}
