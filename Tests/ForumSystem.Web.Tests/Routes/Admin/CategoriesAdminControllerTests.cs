namespace ForumSystem.Web.Tests.Routes.Admin
{
    using ForumSystem.Web.Areas.Administration.Controllers;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class CategoriesAdminControllerTests
    {
        [Fact]
        public void GetIndexShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("Administration/CategoriesAdmin/Index")
                .To<CategoriesAdminController>(c => c.Index(With.Value<int>(1)));

        [Theory]
        [InlineData(1, 1, "Administration/CategoriesAdmin/Approve/1?page=1")]
        [InlineData(6, 2, "Administration/CategoriesAdmin/Approve/6?page=2")]
        [InlineData(15, 3, "Administration/CategoriesAdmin/Approve/15?page=3")]
        public void GetApproveShouldBeRoutedCorrectly(
            int id,
            int page,
            string location)
           => MyRouting
               .Configuration()
               .ShouldMap(location)
               .To<CategoriesAdminController>(c => c.Approve(id, page));
    }
}
