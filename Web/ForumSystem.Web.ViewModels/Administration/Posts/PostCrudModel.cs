namespace ForumSystem.Web.ViewModels.Administration.Posts
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class PostCrudModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string EditTitle => this.Title.Length > 20 ? this.Title.Substring(0, 17) + "..." : this.Title;

        public string CategoryImageUrl { get; set; }
    }
}
