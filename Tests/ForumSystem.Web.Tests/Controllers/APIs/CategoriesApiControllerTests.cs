namespace ForumSystem.Web.Tests.Controllers.APIs
{
    using System.Collections.Generic;
    using System.Linq;

    using ForumSystem.Web.Controllers.APIs;
    using ForumSystem.Web.ViewModels.Posts;

    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

    using static ForumSystem.Web.Tests.Data.CategoiresTestData;

    public class CategoriesApiControllerTests
    {
        [Theory]
        [InlineData(5, "es", 5)]
        [InlineData(8, "3", 1)]
        [InlineData(10, "sas", 0)]
        public void GetSearchShouldReturnCategoriesWithchnameOrDescriptionMarchTheTerm(
            int categoryCount,
            string invalidTerm,
            int expectedCount)
            => MyController<CategoriesApiController>
                .Instance(instance => instance
                    .WithData(GetCategories(categoryCount)))
                .Calling(c => c.Search(invalidTerm))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests()
                    .SpecifyingRoute("search"))
                .AndAlso()
                .ShouldReturn()
                .Ok(ok => ok
                    .WithModelOfType<IEnumerable<CategoryDropDownViewModel>>()
                    .Passing(data =>
                    {
                        data.Count().ShouldBe(expectedCount);
                    }));

        [Theory]
        [InlineData(5, "   ")]
        [InlineData(8, "")]
        [InlineData(10, null)]
        public void GetSearchShouldReturnAllCategoriesIfTermIsNullOrWhitespace(
            int categoryCount,
            string invalidTerm)
            => MyController<CategoriesApiController>
                .Instance(instance => instance
                    .WithData(GetCategories(categoryCount)))
                .Calling(c => c.Search(invalidTerm))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests()
                    .SpecifyingRoute("search"))
                .AndAlso()
                .ShouldReturn()
                .Ok(ok => ok
                    .WithModelOfType<IEnumerable<CategoryDropDownViewModel>>()
                    .Passing(data =>
                    {
                        data.Count().ShouldBe(categoryCount);
                    }));
    }
}
