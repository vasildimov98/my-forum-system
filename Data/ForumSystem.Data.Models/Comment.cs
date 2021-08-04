namespace ForumSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ForumSystem.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    {
        public Comment()
        {
            this.CommentVotes = new HashSet<CommentVote>();
            this.SubComments = new HashSet<Comment>();
        }

        [Required]
        public string Content { get; set; }

        public int? ParentId { get; set; }

        public virtual Comment Parent { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<CommentVote> CommentVotes { get; set; }

        public virtual ICollection<Comment> SubComments { get; set; }
    }
}
