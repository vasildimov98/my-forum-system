namespace ForumSystem.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    public class CategoryInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [RegularExpression(
            @"[A-Z](.*){2,}",
            ErrorMessage = "Name should start with upper case letter!")]
        public string Name { get; set; }

        [Required]
        [MaxLength(3000, ErrorMessage = "Description way too long. Only 3000 letters allowed!")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Image")]
        [RegularExpression(
            @"(?:([^:/?#]+):)?(?://([^/?#]*))?([^?#]*\.(?:jpg|jpeg|gif|png))(?:\?([^#]*))?(?:#(.*))?",
            ErrorMessage = "That is invalid image address. It should end with .jgp, jgep, gif or png!")]
        public string ImageUrl { get; set; }
    }
}
