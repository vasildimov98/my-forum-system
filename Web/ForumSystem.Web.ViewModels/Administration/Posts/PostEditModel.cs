namespace ForumSystem.Web.ViewModels.Administration.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Posts;

    public class PostEditModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [Required]
        [MinLength(50, ErrorMessage = "Content is way to short. Tell me more.")]
        public string Content { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<CategoryDropDownViewModel> Categories { get; set; }
    }
}
