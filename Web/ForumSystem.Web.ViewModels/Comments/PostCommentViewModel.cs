namespace ForumSystem.Web.ViewModels.Comments
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

        public string ImageSrc { get; set; }

        public int PostId { get; set; }

        public int? ParentId { get; set; }

        public int VoteTypeCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                    .CreateMap<Comment, PostCommentViewModel>()
                    .ForMember(x => x.VoteTypeCount, opt =>
                            opt.MapFrom(y => y.CommentVotes.Sum(z => (int)z.Type)));
            configuration
                .CreateMap<Comment, PostCommentViewModel>()
                .ForMember(x => x.ImageSrc, opt =>
                    opt.MapFrom(y => y.User.HasImage ?
                        $"/profileImages/{y.User.ProfileImage.Id}{y.User.ProfileImage.Extention}" :
                        "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png"));
        }
    }
}
