namespace ForumSystem.Web.Tests.Controllers
{
    using System.Linq;

    using ForumSystem.Web.Controllers;
    using ForumSystem.Web.ViewModels.Chat;

    using MyTested.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

    using static ForumSystem.Common.GlobalConstants;
    using static ForumSystem.Web.Tests.Data.CategoiresTestData;

    public class ChatControllerTests
    {
        [Theory]
        [InlineData("TestName1", 5)]
        public void GetLiveChatShouldBeOnlyForAuthorizeUserAndReturnCorrectMessageViewModel(
            string categoryName,
            int count)
            => MyController<ChatsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(GetCategories(1)))
                .Calling(c => c.LiveChat(categoryName))
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
                        .ShouldBe(count);
                    }));

        [Theory]
        [InlineData("InvalidTestName")]
        public void GetLiveChatShouldReturnNotFoundIfThereIsNoSuchCatetegory(
            string invalidName)
            => MyController<ChatsController>
                .Instance(instance => instance
                    .WithData(GetCategories(1)))
                .Calling(c => c.LiveChat(invalidName))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .TempData(data => data
                    .ContainingEntryWithKey(ErrorTitleKey)
                    .ContainingEntryWithValue(ErrorNotFoundTitle)
                    .ContainingEntryWithKey(ErrorMessageKey)
                    .ContainingEntryWithValue(ErrorNotFoundMessage))
               .AndAlso()
               .ShouldReturn()
               .Redirect(red => red
                    .To<HomeController>(c => c.Error()));
    }
}
