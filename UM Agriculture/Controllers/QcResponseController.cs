using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.QcResponse;
using Application.Features.QcResponses.Commands;
using Application.Features.QcResponses.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UM_Agriculture.Extensions;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QcResponseController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [Route("dispatch")]
        public async Task<IActionResult> GetQcResponses([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetDispatchQcResponsesQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQcResponseById(int id)
        {
            var result = await _mediator.Send(new GetQcResponseByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPost]
        public async Task<IActionResult> AddQcResponse([FromBody] AddQcResponseDTO addQcResponseDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new AddQcResponseCommand(userId, addQcResponseDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateQcResponse(int id, [FromBody] UpdateQcResponseDTO updateQcResponseDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new UpdateQcResponseCommand(userId, id, updateQcResponseDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveQcResponse(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleQcResponseActiveCommand(userId, id, false));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreQcResponse(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleQcResponseActiveCommand(userId, id, true));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}
