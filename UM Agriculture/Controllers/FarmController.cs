using Application.DTO.Farm;
using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Features.Farms.Commands;
using Application.Features.Farms.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UM_Agriculture.Extensions;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FarmController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetFarms([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetFarmsQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFarmById(int id)
        {
            var result = await _mediator.Send(new GetFarmByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddFarm([FromBody] AddFarmDTO addFarmDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new AddFarmCommand(userId, addFarmDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFarm(int id, [FromBody] UpdateFarmDTO updateFarmDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new UpdateFarmCommand(userId, id, updateFarmDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveFarm(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleFarmActiveCommand(userId, id, false));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreFarm(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleFarmActiveCommand(userId, id, true));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}
