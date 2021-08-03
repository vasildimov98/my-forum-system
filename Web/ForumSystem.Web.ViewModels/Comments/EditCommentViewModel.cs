namespace ForumSystem.Web.ViewModels.Comments
{
    using Ganss.XSS;

    public class EditCommentViewModel
    {
        public string Content { get; set; }

        public string SanitizeContent =>
            new HtmlSanitizer()
            .Sanitize(this.Content);
    }
}
