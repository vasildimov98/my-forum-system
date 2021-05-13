namespace ForumSystem.Web.ViewModels.Posts
{
    using ForumSystem.Data.Models;
    using ForumSystem.Data.Models.Enums;
    using ForumSystem.Services.Mapping;

    public class PostVotesViewModel : IMapFrom<Vote>
    {
        public VoteType Type { get; set; }
    }
}
