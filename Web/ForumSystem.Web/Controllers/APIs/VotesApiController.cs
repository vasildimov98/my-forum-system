namespace ForumSystem.Web.Controllers.APIs
{
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Votes;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class VotesApiController : ControllerBase
    {
        private readonly IVotesService votesService;
        private readonly UserManager<ApplicationUser> userManager;

        public VotesApiController(
            IVotesService votesService,
            UserManager<ApplicationUser> userManager)
        {
            this.votesService = votesService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<VoteResponseModel>> Post(VoteInpuModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.votesService.VoteAsync(input.Id, userId, input.IsUpVote);
            var votes = await this.votesService.GetAllVotesAsync(input.Id);
            return new VoteResponseModel { VotesCount = votes };
        }

        [HttpPost]
        [Authorize]
        [Route("comment")]
        public async Task<ActionResult<VoteResponseModel>> Post(VoteCommentInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.votesService.VoteCommentAsync(input.Id, userId, input.IsUpVote);
            var votes = await this.votesService.GetAllCommentVotesAsync(input.Id);
            return new VoteResponseModel { VotesCount = votes };
        }
    }
}
