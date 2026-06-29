using Application.DTO.ProductCategory;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.ProductCategories.Commands
{
    /// <summary>Command to update an existing product category.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the product category to update.</param>
    /// <param name="UpdateProductCategoryDTO">The updated product category data.</param>
    public record UpdateProductCategoryCommand(int? UserId, int Id, UpdateProductCategoryDTO UpdateProductCategoryDTO) : IRequest<Result<object>>;
    public class UpdateProductCategoryCommandHandler(IProductCategoryRepository productCategoryRepository, IUserRepository userRepository) : IRequestHandler<UpdateProductCategoryCommand, Result<object>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository = productCategoryRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == null)
            {
                return Result<object>.Failure("User is not signed in", HttpStatusCode.Unauthorized);
            }

            var existingUser = await _userRepository.GetByIdAsync(request.UserId.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existing = await _productCategoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existing == null)
            {
                return Result<object>.Failure("Product category not found", System.Net.HttpStatusCode.NotFound);
            }

            if (!string.IsNullOrEmpty(request.UpdateProductCategoryDTO.Name))
            {
                var dup = await _productCategoryRepository.CheckDuplicateAsync(request.Id, request.UpdateProductCategoryDTO.Name, cancellationToken);
                if (dup)
                {
                    return Result<object>.Failure("Product category name already exists", System.Net.HttpStatusCode.BadRequest);
                }

                existing.Name = request.UpdateProductCategoryDTO.Name;
            }

            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedById = existingUser.Id;

            await _productCategoryRepository.UpdateAsync(existing, cancellationToken);

            return Result<object>.Success(existing.Id, "Product category updated successfully");
        }
    }
}
