namespace ForumSystem.Data.Models
{
    using ForumSystem.Data.Common.Models;

    public class Message : BaseModel<int>
    {
        public string Content { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
