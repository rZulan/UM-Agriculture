using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.QcSection;
using Application.Features.QcSections.Commands;
using Application.Features.QcSections.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UM_Agriculture.Extensions;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QcSectionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetQcSections([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetQcSectionsQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQcSectionById(int id)
        {
            var result = await _mediator.Send(new GetQcSectionByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddQcSection([FromBody] AddQcSectionDTO addQcSectionDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new AddQcSectionCommand(userId, addQcSectionDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateQcSection(int id, [FromBody] UpdateQcSectionDTO updateQcSectionDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new UpdateQcSectionCommand(userId, id, updateQcSectionDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveQcSection(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleQcSectionActiveCommand(userId, id, false));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreQcSection(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleQcSectionActiveCommand(userId, id, true));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}

