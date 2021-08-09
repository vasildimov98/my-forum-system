namespace ForumSystem.Web.Tests.Controllers
{
    using ForumSystem.Web.Controllers;
    using ForumSystem.Web.ViewModels.Chat;
    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using System.Linq;
    using Xunit;

    using static ForumSystem.Web.Tests.Data.CategoiresTestData;

    public class ChatControllerTests
    {
        [Fact]
        public void GetLiveChatShouldBeOnluForAuthorizeUserAndReturnCorrectMessageViewModel()
            => MyController<ChatsController>
                .Instance(instance => instance
                    .WithData(GetCategories(1)))
                .Calling(c => c.LiveChat("TestedName1"))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<LiveChatViewModel>()
                    .Passing(liveChatViewModel =>
                    {
                        liveChatViewModel.Messages
                        .Count()
                        .ShouldBe(5);
                    }));
    }
}
