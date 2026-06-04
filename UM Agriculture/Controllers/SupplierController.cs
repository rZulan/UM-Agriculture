using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.Supplier;
using Application.Features.Suppliers.Commands;
using Application.Features.Suppliers.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UM_Agriculture.Extensions;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetSuppliers([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetSuppliersQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var result = await _mediator.Send(new GetSupplierByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSupplier([FromBody] AddSupplierDTO addSupplierDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new AddSupplierCommand(userId, addSupplierDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierDTO updateSupplierDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new UpdateSupplierCommand(userId, id, updateSupplierDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveSupplier(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleSupplierActiveCommand(userId, id, false));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreSupplier(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleSupplierActiveCommand(userId, id, true));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}
