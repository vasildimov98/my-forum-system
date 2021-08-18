namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PostInputModel
    {
        [Required]
        [MaxLength(300)]
        [RegularExpression("^[^_]*$", ErrorMessage = "Underscore is not allowed in the title.")]
        public string Title { get; set; }

        [Required]
        [MinLength(50, ErrorMessage ="Content is way to short. Tell me more.")]
        public string Content { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryDropDownViewModel> Categories { get; set; }

        public string Selected { get; set; }
    }
}
