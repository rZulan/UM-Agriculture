using Application.DTO.Department;
using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Features.Departments.Commands;
using Application.Features.Departments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UM_Agriculture.Extensions;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetDepartments([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetDepartmentsQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var result = await _mediator.Send(new GetDepartmentByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentDTO addDepartmentDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new AddDepartmentCommand(userId, addDepartmentDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentDTO updateDepartmentDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new UpdateDepartmentCommand(userId, id, updateDepartmentDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveDepartment(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleDepartmentActiveCommand(userId, id, false));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreDepartment(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleDepartmentActiveCommand(userId, id, true));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}
