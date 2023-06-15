using Application.Activities;
using Application.Photos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PhotosController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Add.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (string id)
        {
            return HandleResult(await Mediator.Send(new Application.Photos.Delete.Command { Id = id }));   
        }

        [HttpPost("setMain/{id}")]
        public async Task<ActionResult> SetMain (string id)
        {
            return HandleResult(await Mediator.Send(new SetMain.Command { Id = id }));  
        }
    }
}
