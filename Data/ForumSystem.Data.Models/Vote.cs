namespace ForumSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using ForumSystem.Data.Common.Models;
    using ForumSystem.Data.Models.Enums;

    public class Vote : BaseModel<int>
    {
        public int PostId { get; set; }

        public Post Post { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public VoteType Type { get; set; }
    }
}
