using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.Pond;
using Application.Features.Ponds.Commands;
using Application.Features.Ponds.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UM_Agriculture.Extensions;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PondController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetPonds([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetPondsQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPondById(int id)
        {
            var result = await _mediator.Send(new GetPondByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPond([FromBody] AddPondDTO addPondDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new AddPondCommand(userId, addPondDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePond(int id, [FromBody] UpdatePondDTO updatePondDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new UpdatePondCommand(userId, id, updatePondDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchivePond(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new TogglePondActiveCommand(userId, id, false));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestorePond(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new TogglePondActiveCommand(userId, id, true));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}
