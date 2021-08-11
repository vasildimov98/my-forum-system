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
    }
}
