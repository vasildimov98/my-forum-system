namespace ForumSystem.Web.ViewModels.Home
{
    using ForumSystem.Data.Models;
    using ForumSystem.Data.Models.Enums;
    using ForumSystem.Services.Mapping;

    public class HomePostVotesViewModel : IMapFrom<Vote>
    {
        public VoteType Type { get; set; }
    }
}
