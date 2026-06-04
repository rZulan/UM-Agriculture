using Application.DTO.HorticultureClass;
using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Features.HorticultureClasses.Commands;
using Application.Features.HorticultureClasses.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UM_Agriculture.Extensions;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HorticultureClassController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetHorticultureClasses([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetHorticultureClassesQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHorticultureClassById(int id)
        {
            var result = await _mediator.Send(new GetHorticultureClassByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddHorticultureClass([FromBody] AddHorticultureClassDTO addHorticultureClassDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new AddHorticultureClassCommand(userId, addHorticultureClassDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateHorticultureClass(int id, [FromBody] UpdateHorticultureClassDTO updateHorticultureClassDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new UpdateHorticultureClassCommand(userId, id, updateHorticultureClassDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveHorticultureClass(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleHorticultureClassActiveCommand(userId, id, false));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreHorticultureClass(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleHorticultureClassActiveCommand(userId, id, true));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}
