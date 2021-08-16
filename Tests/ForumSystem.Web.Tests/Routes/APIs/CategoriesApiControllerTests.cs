namespace ForumSystem.Web.Tests.Routes.APIs
{
    using ForumSystem.Web.Controllers.APIs;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class CategoriesApiControllerTests
    {
        [Theory]
        [InlineData("/api/categories/search", null)]
        [InlineData("/api/categories/search?term=123", "123")]
        public void GetSearchContentShouldBeRoutedCorrectly(
            string location,
            string term)
           => MyRouting
               .Configuration()
               .ShouldMap(location)
               .To<CategoriesApiController>(c => c.Search(term));
    }
}
