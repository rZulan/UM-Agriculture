using Application.DTO.ProductCategory;
using Application.Interfaces;
using Application.Results;
using Domain.Entities.Masterlist;
using MediatR;
using System.Net;

namespace Application.Features.ProductCategories.Commands
{
    /// <summary>Command to create a new product category.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="AddProductCategoryDTO">The product category data to be created.</param>
    public record AddProductCategoryCommand(int? UserId, AddProductCategoryDTO AddProductCategoryDTO) : IRequest<Result<object>>;
    public class AddProductCategoryCommandHandler(IProductCategoryRepository productCategoryRepository, IUserRepository userRepository) : IRequestHandler<AddProductCategoryCommand, Result<object>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository = productCategoryRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(AddProductCategoryCommand request, CancellationToken cancellationToken)
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

            var existing = await _productCategoryRepository.GetByIdAsync(request.AddProductCategoryDTO.Name.GetHashCode(), cancellationToken);

            if (existing != null)
            {
                return Result<object>.Failure("Product category with the same name already exists", HttpStatusCode.Conflict);
            }

            var productCategory = new ProductCategory
            {
                Name = request.AddProductCategoryDTO.Name,
                CreatedAt = DateTime.UtcNow,
                CreatedById = existingUser.Id
            };

            await _productCategoryRepository.AddAsync(productCategory, cancellationToken);

            return Result<object>.Success(productCategory.Id, "Product category created successfully", HttpStatusCode.Created);
        }
    }
}
