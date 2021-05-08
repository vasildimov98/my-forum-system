namespace ForumSystem.Web.ViewModels.Home
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;

    using AutoMapper;

    using ForumSystem.Data.Models;
    using ForumSystem.Data.Models.Enums;
    using ForumSystem.Services.Mapping;

    public class HomePostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int CommentsCount { get; set; }

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

        public string UserUserName { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Post, HomePostViewModel>()
                .ForMember(x => x.Likes, opt =>
                {
                    opt.MapFrom(y => y.Votes
                        .Count());
                });

            configuration
                .CreateMap<Post, HomePostViewModel>()
                .ForMember(x => x.Dislikes, opt =>
                {
                    opt.MapFrom(y => y.Votes
                        .Count());
                });
        }
    }
}
