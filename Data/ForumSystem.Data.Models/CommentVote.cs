namespace ForumSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using ForumSystem.Data.Common.Models;
    using ForumSystem.Data.Models.Enums;

    public class CommentVote : BaseModel<int>
    {
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public VoteType Type { get; set; }
    }
}
