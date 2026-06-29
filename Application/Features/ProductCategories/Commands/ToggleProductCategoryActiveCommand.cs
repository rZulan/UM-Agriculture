using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.ProductCategories.Commands
{
    /// <summary>Command to activate or deactivate a product category.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the product category to toggle.</param>
    /// <param name="IsActive">The desired active state.</param>
    public record ToggleProductCategoryActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleProductCategoryActiveCommandHandler(IProductCategoryRepository productCategoryRepository, IUserRepository userRepository) : IRequestHandler<ToggleProductCategoryActiveCommand, Result<object>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository = productCategoryRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleProductCategoryActiveCommand request, CancellationToken cancellationToken)
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
                return Result<object>.Failure("Product category not found");
            }

            if (request.IsActive && existing.IsActive)
            {
                return Result<object>.Failure("Product category is already active");
            }

            if (!request.IsActive && !existing.IsActive)
            {
                return Result<object>.Failure("Product category is already archived");
            }

            existing.IsActive = request.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedById = existingUser.Id;

            await _productCategoryRepository.UpdateAsync(existing, cancellationToken);

            var status = existing.IsActive ? "restored" : "archived";

            return Result<object>.Success(existing.Id, $"Product category {status} successfully");
        }
    }
}
