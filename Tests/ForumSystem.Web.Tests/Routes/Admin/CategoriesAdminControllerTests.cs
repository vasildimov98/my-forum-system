namespace ForumSystem.Web.Tests.Routes.Admin
{
    using ForumSystem.Web.Areas.Administration.Controllers;
    using ForumSystem.Web.ViewModels.Administration.Categories;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class CategoriesAdminControllerTests
    {
        [Fact]
        public void GetIndexShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("Administration/Categories/Index")
                .To<CategoriesController>(c => c.Index(With.Value<int>(1)));

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
                .To<CategoriesController>(c => c.Edit(With.Value<int>(1)));

        [Fact]
        public void PostEditShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("Category/Edit/1"))
                .To<CategoriesController>(c => c.Edit(With.Any<int>()));

        [Fact]
        public void GetDeleteShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("Category/Delete/1")
                .To<CategoriesController>(c => c.Delete(With.Value<int>(1)));

        [Fact]
        public void PostDeleteShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("Category/Delete/1"))
                .To<CategoriesController>(c => c.DeleteConfirmed(With.Value<int>(1)));
    }
}
