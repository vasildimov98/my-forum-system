namespace ForumSystem.Web.ViewModels.Posts
{
    using System.ComponentModel.DataAnnotations;

    public class PostInputModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int CategoryId { get; set; }
    }
}
