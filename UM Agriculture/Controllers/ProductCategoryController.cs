using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Features.ProductCategories.Commands;
using Application.Features.ProductCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UM_Agriculture.Extensions;

namespace UM_Agriculture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductCategoryController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetProductCategories([FromQuery] GenericFiltersDTO genericFiltersDTO, [FromQuery] Sort sort)
        {
            var result = await _mediator.Send(new GetProductCategoriesQuery(genericFiltersDTO, sort));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductCategoryById(int id)
        {
            var result = await _mediator.Send(new GetProductCategoryByIdQuery(id));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProductCategory([FromBody] Application.DTO.ProductCategory.AddProductCategoryDTO addProductCategoryDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new AddProductCategoryCommand(userId, addProductCategoryDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductCategory(int id, [FromBody] Application.DTO.ProductCategory.UpdateProductCategoryDTO updateProductCategoryDTO)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new UpdateProductCategoryCommand(userId, id, updateProductCategoryDTO));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveProductCategory(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleProductCategoryActiveCommand(userId, id, false));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }

        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreProductCategory(int id)
        {
            var userId = this.GetCurrentUserId();

            var result = await _mediator.Send(new ToggleProductCategoryActiveCommand(userId, id, true));

            return StatusCode(result.StatusCode!.Value.GetHashCode(), result);
        }
    }
}