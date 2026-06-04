using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.QcAnswer;
using Application.Features.QcAnswers.Commands;
using Application.Features.QcAnswers.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UM_Agriculture.Extensions;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QcAnswerController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetQcAnswers([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetQcAnswersQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQcAnswerById(int id)
        {
            var result = await _mediator.Send(new GetQcAnswerByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPost]
        public async Task<IActionResult> AddQcAnswer([FromBody] AddQcAnswerDTO addQcAnswerDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new AddQcAnswerCommand(userId, addQcAnswerDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateQcAnswer(int id, [FromBody] UpdateQcAnswerDTO updateQcAnswerDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new UpdateQcAnswerCommand(userId, id, updateQcAnswerDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveQcAnswer(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleQcAnswerActiveCommand(userId, id, false));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreQcAnswer(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleQcAnswerActiveCommand(userId, id, true));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}
