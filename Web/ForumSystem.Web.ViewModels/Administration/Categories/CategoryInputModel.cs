namespace ForumSystem.Web.ViewModels.Administration.Categories
{
    using System.ComponentModel.DataAnnotations;

    public class CategoryInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(35)]
        [RegularExpression(
            @"[A-Z](.*){2,}",
            ErrorMessage = "Name should start with upper case letter!")]
        public string Name { get; set; }

        [Required]
        [MinLength(50)]
        public string Description { get; set; }

        [Required]
        [RegularExpression(
            @"(?:([^:/?#]+):)?(?://([^/?#]*))?([^?#]*\.(?:jpg|jpeg|gif|png))(?:\?([^#]*))?(?:#(.*))?",
            ErrorMessage = "That is invalid image address. It should end with .jgp, jgep, gif or png!")]
        public string Image { get; set; }
    }
}
