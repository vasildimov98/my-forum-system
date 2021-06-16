namespace ForumSystem.Web.ViewModels.Chat
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class LiveChatViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }
    }
}
