namespace ForumSystem.Web.Tests.Routes
{
    using ForumSystem.Web.Controllers;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class ProfilesControllerTests
    {
        [Fact]
        public void GetByUsernameShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/User/TestUsername/1")
                .To<ProfilesController>(c => c.ByUsername("TestUsername", 1, null));
    }
}
