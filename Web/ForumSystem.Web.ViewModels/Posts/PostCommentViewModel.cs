namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    using AutoMapper;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using Ganss.XSS;

    public class PostCommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string SanitizeContent => new HtmlSanitizer().Sanitize(this.Content);

        public string FormCommentId => $"commentBox{this.Id}";

        public string UserUserName { get; set; }

        public int PostId { get; set; }

        public int? ParentId { get; set; }

        public IEnumerable<PostCommentViewModel> SubComments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
        }
    }
}
