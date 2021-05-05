namespace ForumSystem.Web.ViewModels.Home
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class HomePostViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ShortContent
        {
            get
            {
                var content = WebUtility.HtmlDecode(Regex.Replace(this.Content, @"<[^>]*>", string.Empty));
                return content.Length > 400
                        ? content.Substring(0, 400) + $"... <a href=\"/Posts/ById/{this.Id}\" class=\"text-secondary\">read more</a>"
                        : content + $"... <a href=\"/Posts/ById/{this.Id}\" class=\"text-secondary\">read more</a>";
            }
        }

        public DateTime CreatedOn { get; set; }

        public string CategoryImage { get; set; }
    }
}
