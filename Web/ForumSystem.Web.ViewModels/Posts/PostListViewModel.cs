namespace ForumSystem.Web.ViewModels.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text.RegularExpressions;

    using AutoMapper;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using Ganss.XSS;

    public class PostListViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url => $"/Post/{this.Id}/{this.Title.ToLower().Replace(" ", "_")}";

        public string PostCreatorHref => $"/User/{this.UserUserName}/1";

        public string Content { get; set; }

        public int CountOfComments { get; set; }

        public string ShortContent
        {
            get
            {
                var content = WebUtility.HtmlDecode(
                    Regex
                    .Replace(
                        new HtmlSanitizer()
                    .Sanitize(this.Content), @"<[^>]*>", string.Empty));
                return content.Length > 400
                        ? content.Substring(0, 400) + $"... <a href={this.Url} class=\"text-secondary\">read more</a>"
                        : content + $"... <a href={this.Url} class=\"text-secondary\">read more</a>";
            }
        }

        public DateTime CreatedOn { get; set; }

        public string CategoryImageUrl { get; set; }

        public string UserUserName { get; set; }

        public IEnumerable<PostVotesViewModel> Votes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Post, PostListViewModel>()
                .ForMember(x => x.CountOfComments, y =>
                    y.MapFrom(x => x.Comments.Count));
        }
    }
}
