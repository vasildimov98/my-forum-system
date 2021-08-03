namespace ForumSystem.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    public class CommentInputModel
    {
        public int PostId { get; set; }

        public int? ParentId { get; set; }

        [Required]
        [MinLength(3)]
        public string Content { get; set; }
    }
}
