namespace ForumSystem.Web.Tests.Routes
{
    using ForumSystem.Web.Controllers;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class HomeControllerTests
    {
        [Fact]
        public void GetPrivacyShouldBeRoutedCorrectly()
            => MyRouting
                    .Configuration()
                    .ShouldMap("/Home/Privacy")
                    .To<HomeController>(c => c.Privacy());
    }
}
