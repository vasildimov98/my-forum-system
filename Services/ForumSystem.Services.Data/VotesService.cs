namespace ForumSystem.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Data.Models.Enums;
    using Microsoft.EntityFrameworkCore;

    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> votesRepository;
        private readonly IRepository<CommentVote> commentVotesRepository;

        public VotesService(
            IRepository<Vote> votesRepository,
            IRepository<CommentVote> commentVotesRepository)
        {
            this.votesRepository = votesRepository;
            this.commentVotesRepository = commentVotesRepository;
        }

        public async Task<int> GetAllVotesAsync(int postId)
            => await this.votesRepository
                .All()
                .Where(x => x.PostId == postId)
                .SumAsync(x => (int)x.Type);

        public async Task<int> GetAllCommentVotesAsync(int commentId)
            => await this.commentVotesRepository
                .All()
                .Where(x => x.CommentId == commentId)
                .SumAsync(x => (int)x.Type);

        public async Task VoteAsync(int postId, string userId, bool isUpVote)
        {
            var vote = await this.votesRepository
                    .All()
                    .FirstOrDefaultAsync(x => x.PostId == postId
                                    && x.UserId == userId);

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

        public async Task VoteCommentAsync(int commentId, string userId, bool isUpVote)
        {
            var vote = await this.commentVotesRepository
                    .All()
                    .FirstOrDefaultAsync(x => x.CommentId == commentId
                                    && x.UserId == userId);

            var typeVote = isUpVote ? VoteType.Like : VoteType.Dislike;

            if (vote == null)
            {
                vote = new CommentVote
                {
                    CommentId = commentId,
                    UserId = userId,
                    Type = typeVote,
                };

                await this.commentVotesRepository.AddAsync(vote);
                await this.commentVotesRepository.SaveChangesAsync();
            }

            if (vote != null
                && vote.Type != typeVote)
            {
                vote.Type = typeVote;
                await this.commentVotesRepository.SaveChangesAsync();
            }
        }
    }
}
