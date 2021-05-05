namespace ForumSystem.Web.ViewModels.Posts
{
    using System;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    using Ganss.XSS;

    public class PostViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string SanitizeContent => new HtmlSanitizer().Sanitize(this.Content);

        public string CategoryName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserUserName { get; set; }

        public DateTime UserCreatedOn { get; set; }

        public int UserPostsCount { get; set; }

        public string CategoryImage { get; set; }
    }
}
