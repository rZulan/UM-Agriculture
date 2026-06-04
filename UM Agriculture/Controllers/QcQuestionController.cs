using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.QcQuestion;
using Application.Features.QcQuestions.Commands;
using Application.Features.QcQuestions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UM_Agriculture.Extensions;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QcQuestionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetQcQuestions([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetQcQuestionsQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQcQuestionById(int id)
        {
            var result = await _mediator.Send(new GetQcQuestionByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddQcQuestion([FromBody] AddQcQuestionDTO addQcQuestionDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new AddQcQuestionCommand(userId, addQcQuestionDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateQcQuestion(int id, [FromBody] UpdateQcQuestionDTO updateQcQuestionDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new UpdateQcQuestionCommand(userId, id, updateQcQuestionDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveQcQuestion(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleQcQuestionActiveCommand(userId, id, false));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreQcQuestion(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleQcQuestionActiveCommand(userId, id, true));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}

