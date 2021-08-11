namespace ForumSystem.Web.Tests.Component
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ForumSystem.Web.Components;
    using ForumSystem.Web.ViewModels.Components;

    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

    using static ForumSystem.Common.GlobalConstants;
    using static ForumSystem.Web.Tests.Data.CategoiresTestData;

    public class FamousCategoriesComponentTests
    {
        [Theory]
        [InlineData(5, "TestName10", "TestName10", "TestName10", "TestName10", "TestName10")]
        public void ViewComponentShouldReturnTopFiveCateogiresWithMostPostsAndToCashThem(
            int countOfCategory,
            string firstCategoryName,
            string secondCategoryName,
            string thirdCategoryName,
            string fourthCategoryName,
            string fifthCategoryName)
            => MyMvc
                .ViewComponent<FamousCategoriesViewComponent>()
                .WithData(data => data
                    .WithEntities(entities => entities
                        .AddRange(GetFamousCategories())))
                .InvokedWith(vc => vc.InvokeAsync())
                .ShouldHave()
                .MemoryCache(cashe => cashe
                    .ContainingEntry(entry => entry
                        .WithKey(FamousCategoriesKey)
                        .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(30))
                        .WithValueOfType<List<FamousCategoryViewModel>>()))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<FamousCategoriesListModel>()
                    .Passing(model =>
                    {
                        model.Categories.Count().ShouldBe(countOfCategory);
                        model.Categories.Any(c => c.Name.Equals(firstCategoryName));
                        model.Categories.Any(c => c.Name.Equals(secondCategoryName));
                        model.Categories.Any(c => c.Name.Equals(thirdCategoryName));
                        model.Categories.Any(c => c.Name.Equals(fourthCategoryName));
                        model.Categories.Any(c => c.Name.Equals(fifthCategoryName));
                    }));

        [Theory]
        [InlineData(3, "TestName1", "TestName2", "TestName3")]
        public void ViewComponentShouldReturnAllCateogiresIfThereAreLessThenfiveInTheDatabase(
            int countOfCategory,
            string firstCategoryName,
            string secondCategoryName,
            string thirdCategoryName)
            => MyMvc
                .ViewComponent<FamousCategoriesViewComponent>()
                .WithData(data => data
                    .WithEntities(entities => entities
                        .AddRange(GetFamousCategories(countOfCategory))))
                .InvokedWith(vc => vc.InvokeAsync())
                .ShouldHave()
                .MemoryCache(cashe => cashe
                    .ContainingEntry(entry => entry
                        .WithKey(FamousCategoriesKey)
                        .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(30))
                        .WithValueOfType<List<FamousCategoryViewModel>>()))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<FamousCategoriesListModel>()
                    .Passing(model =>
                    {
                        model.Categories.Count().ShouldBe(countOfCategory);
                        model.Categories.Any(c => c.Name.Equals(firstCategoryName));
                        model.Categories.Any(c => c.Name.Equals(secondCategoryName));
                        model.Categories.Any(c => c.Name.Equals(thirdCategoryName));
                    }));
    }
}
