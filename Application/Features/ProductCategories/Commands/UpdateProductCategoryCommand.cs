using Application.DTO.ProductCategory;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.ProductCategories.Commands
{
    public record UpdateProductCategoryCommand(int? UserId, int Id, UpdateProductCategoryDTO UpdateProductCategoryDTO) : IRequest<Result<object>>;
    public class UpdateProductCategoryCommandHandler(IProductCategoryRepository productCategoryRepository, IUserRepository userRepository) : IRequestHandler<UpdateProductCategoryCommand, Result<object>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository = productCategoryRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

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
