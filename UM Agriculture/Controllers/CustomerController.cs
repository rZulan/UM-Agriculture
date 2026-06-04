using Application.DTO.Customer;
using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UM_Agriculture.Extensions;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetCustomers([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetCustomersQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var result = await _mediator.Send(new GetCustomerByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCustomer([FromBody] AddCustomerDTO addCustomerDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new AddCustomerCommand(userId, addCustomerDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDTO updateCustomerDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new UpdateCustomerCommand(userId, id, updateCustomerDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveCustomer(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleCustomerActiveCommand(userId, id, false));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreCustomer(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleCustomerActiveCommand(userId, id, true));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}
