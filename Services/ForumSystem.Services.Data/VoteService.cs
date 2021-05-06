namespace ForumSystem.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Data.Models.Enums;
    using Microsoft.EntityFrameworkCore;

    public class VoteService : IVotesService
    {
        private readonly IRepository<Vote> votesRepository;

        public VoteService(IRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public async Task<int> GetAllVotesAsync(int postId)
            => await this.votesRepository
                .All()
                .Where(x => x.PostId == postId)
                .SumAsync(x => (int)x.Type);

        public async Task VoteAsync(int postId, string userId, bool isUpVote)
        {
            var vote = await this.votesRepository
                    .All()
                    .FirstOrDefaultAsync(x => x.PostId == postId);

            var typeVote = isUpVote ? VoteType.Like : VoteType.Dislike;

            if (vote == null)
            {
                vote = new Vote
                {
                    PostId = postId,
                    UserId = userId,
                    Type = typeVote,
                };

                await this.votesRepository.AddAsync(vote);
                await this.votesRepository.SaveChangesAsync();
            }

            if (vote != null
                && vote.Type != typeVote)
            {
                vote.Type = typeVote;
                await this.votesRepository.SaveChangesAsync();
            }
        }
    }
}
