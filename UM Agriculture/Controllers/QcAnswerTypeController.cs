using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Features.QcAnswerTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QcAnswerTypeController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetQcAnswerTypes([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetQcAnswerTypesQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQcAnswerTypeById(int id)
        {
            var result = await _mediator.Send(new GetQcAnswerTypeByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}
