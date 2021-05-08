namespace ForumSystem.Web.ViewModels.Posts
{
    using System;
    using System.Linq;

    using AutoMapper;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using Ganss.XSS;

    public class PostCommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string VotesCountId => $"votesCount{this.Id}";

        public string SanitizeContent => new HtmlSanitizer().Sanitize(this.Content);

        public DateTime CreatedOn { get; set; }

        public string FormCommentId => $"commentBox{this.Id}";

        public string UserUserName { get; set; }

        public int PostId { get; set; }

        public int? ParentId { get; set; }

        public int VoteTypeCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                    .CreateMap<Comment, PostCommentViewModel>()
                    .ForMember(x => x.VoteTypeCount, y =>
                            y.MapFrom(x => x.CommentVotes.Sum(z => (int)z.Type)));
        }
    }
}
