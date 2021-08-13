namespace ForumSystem.Web.Tests.Routes
{
    using ForumSystem.Web.Controllers;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class ChatControllerTests
    {
        [Fact]
        public void GetLiveChatShouldBeRoutedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Chat/Test-Name")
                .To<ChatsController>(c => c.LiveChat(With
                                                        .Value<string>("Test-Name")));
    }
}
