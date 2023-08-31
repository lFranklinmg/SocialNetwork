using Application.Activities;
using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new Application.Profiles.Detail.Query { Username = username }));
        }
        [HttpGet("userActivities/{username}")]
        public async Task<IActionResult> GetActivities(string predicate, string userName)
        {
            return HandleResult(await Mediator.Send(new ListActivities.Query { Username = userName, Predicate = predicate }));
        }
    }
}
