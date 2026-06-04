using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Features.QcTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QcTypeController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetQcTypes([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetQcTypesQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQcTypeById(int id)
        {
            var result = await _mediator.Send(new GetQcTypeByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}
