namespace ForumSystem.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    using Ganss.XSS;

    public class CategoryEditModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(35)]
        [RegularExpression(
              @"[A-Z](.*){2,}",
              ErrorMessage = "Name should start with upper case letter!")]
        public string Name { get; set; }

        [Required]
        [MaxLength(350, ErrorMessage = "Description way too long. Only 350 letters allowed!")]
        public string Description { get; set; }

        public string SanitizeDescription => new HtmlSanitizer().Sanitize(Description);

        [Required]
        [Display(Name = "Image")]
        [RegularExpression(
            @"(?:([^:/?#]+):)?(?://([^/?#]*))?([^?#]*\.(?:jpg|jpeg|gif|png))(?:\?([^#]*))?(?:#(.*))?",
            ErrorMessage = "That is invalid image address. It should end with .jgp, jgep, gif or png!")]
        public string ImageUrl { get; set; }
    }
}
