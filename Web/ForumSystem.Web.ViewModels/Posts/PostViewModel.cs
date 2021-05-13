namespace ForumSystem.Web.ViewModels.Posts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    using Ganss.XSS;

    public class PostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string VotesCountId => $"votesCount{this.Id}";

        public string SanitizeContent => new HtmlSanitizer().Sanitize(this.Content);

        public string CategoryName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserUserName { get; set; }

        public string LoggedInUserName { get; set; }

        public DateTime UserCreatedOn { get; set; }

        public int UserPostsCount { get; set; }

        public string CategoryImageUrl { get; set; }

        public int VoteTypeCount { get; set; }

        public IEnumerable<PostCommentViewModel> Comments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                    .CreateMap<Post, PostViewModel>()
                    .ForMember(x => x.VoteTypeCount, y =>
                            y.MapFrom(x => x.Votes.Sum(z => (int)z.Type)));
        }
    }
}
