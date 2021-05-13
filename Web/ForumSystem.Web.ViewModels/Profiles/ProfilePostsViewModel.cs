namespace ForumSystem.Web.ViewModels.Profiles
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text.RegularExpressions;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Posts;

    public class ProfilePostsViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url => $"/Post/{this.Id}/{this.Title.ToLower().Replace(" ", "-")}";

        public string Content { get; set; }

        public int CommentsCount { get; set; }

        public string ShortContent
        {
            get
            {
                var content = WebUtility.HtmlDecode(Regex.Replace(Content, @"<[^>]*>", string.Empty));
                return content.Length > 400
                        ? content.Substring(0, 400) + $"... <a href={this.Url} class=\"text-secondary\">read more</a>"
                        : content + $"... <a href={this.Url} class=\"text-secondary\">read more</a>";
            }
        }

        public DateTime CreatedOn { get; set; }

        public string CategoryImageUrl { get; set; }

        public string UserUserName { get; set; }

        public IEnumerable<PostVotesViewModel> Votes { get; set; }
    }
}
