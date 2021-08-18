namespace ForumSystem.Web.Tests.Controllers
{
    using ForumSystem.Web.Controllers;
    using ForumSystem.Web.ViewModels;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class HomeControllerTests
    {
        [Fact]
        public void GetPrivacyShouldReturnCorrectView()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Privacy())
                .ShouldReturn()
                .View();

        [Fact]
        public void GetErrorShouldReturnCorrectView()
           => MyController<HomeController>
               .Instance()
               .Calling(c => c.Error())
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                    .CachingResponse(0))
               .AndAlso()
               .ShouldReturn()
               .View();
    }
}
