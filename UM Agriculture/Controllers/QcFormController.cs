using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Features.QcForms.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QcFormController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetQcForms([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetQcFormsQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQcFormById(int id)
        {
            var result = await _mediator.Send(new GetQcFormByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}
