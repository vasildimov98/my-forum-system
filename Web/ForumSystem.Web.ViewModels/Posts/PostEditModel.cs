namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using Ganss.XSS;

    public class PostEditModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        public string UrlTitle => this.Title.Replace(" ", "_");

        public string Content { get; set; }

        [Required]
        [Display(Name = "Content")]
        [MinLength(50, ErrorMessage = "Content is way to short. Tell me more.")]
        public string SanitizeContent => new HtmlSanitizer().Sanitize(this.Content);

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryImageUrl { get; set; }

        public int CommentsCount { get; set; }

        public bool IsFromAdminPanel { get; set; }

        public IEnumerable<CategoryDropDownViewModel> Categories { get; set; }

        public int FromPage { get; set; }
    }
}
