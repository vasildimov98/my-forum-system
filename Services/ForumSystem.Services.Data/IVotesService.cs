namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        public Task VoteAsync(int postId, string userId, bool isUpVote);

        public Task VoteCommentAsync(int commentId, string userId, bool isUpVote);

        public Task<int> GetAllVotesAsync(int postId);

        public Task<int> GetAllCommentVotesAsync(int commentId);
    }
}
