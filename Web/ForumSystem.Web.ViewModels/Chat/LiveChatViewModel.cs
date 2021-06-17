namespace ForumSystem.Web.ViewModels.Chat
{
    using System.Collections.Generic;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class LiveChatViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public IEnumerable<MessageViewModel> Messages { get; set; }
    }
}
