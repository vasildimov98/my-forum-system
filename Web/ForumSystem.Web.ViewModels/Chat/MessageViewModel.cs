namespace ForumSystem.Web.ViewModels.Chat
{
    public class MessageViewModel
    {
        public string User { get; set; }

        public string Content { get; set; }

        public bool IsCurrentUser { get; set; }
    }
}
