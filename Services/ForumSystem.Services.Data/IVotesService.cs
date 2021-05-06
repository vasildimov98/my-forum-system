namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        public Task VoteAsync(int postId, string userId, bool isUpVote);

        public Task<int> GetAllVotesAsync(int postId);
    }
}
