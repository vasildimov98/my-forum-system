namespace ForumSystem.Web.ViewModels.Posts
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using Ganss.XSS;

    public class PostCommentViewModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string SanitizeContent => new HtmlSanitizer().Sanitize(this.Content);

        public string FormCommentId => $"commentBox{this.Id}";

        public string UserUserName { get; set; }

        public int PostId { get; set; }
    }
}
