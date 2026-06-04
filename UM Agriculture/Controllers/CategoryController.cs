using Application.DTO.Category;
using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UM_Agriculture.Extensions;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetCategoriesQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDTO addCategoryDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new AddCategoryCommand(userId, addCategoryDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO updateCategoryDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new UpdateCategoryCommand(userId, id, updateCategoryDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveCategory(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleCategoryActiveCommand(userId, id, false));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreCategory(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleCategoryActiveCommand(userId, id, true));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}
