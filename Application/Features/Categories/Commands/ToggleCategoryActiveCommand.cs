using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Categories.Commands
{
    /// <summary>Command to activate or deactivate a category.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the category to toggle.</param>
    /// <param name="IsActive">The desired active state.</param>
    public record ToggleCategoryActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleCategoryActiveCommandHandler(ICategoryRepository categoryRepository, IUserRepository userRepository) : IRequestHandler<ToggleCategoryActiveCommand, Result<object>>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleCategoryActiveCommand request, CancellationToken cancellationToken)
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

            var existingCategory = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingCategory == null)
            {
                return Result<object>.Failure("Category not found");
            }

            if (request.IsActive && existingCategory.IsActive)
            {
                return Result<object>.Failure("Category is already active");
            }

            if (!request.IsActive && !existingCategory.IsActive)
            {
                return Result<object>.Failure("Category is already archived");
            }

            existingCategory.IsActive = request.IsActive;
            existingCategory.UpdatedAt = DateTime.UtcNow;
            existingCategory.UpdatedById = existingUser.Id;

            await _categoryRepository.UpdateAsync(existingCategory, cancellationToken);

            var status = existingCategory.IsActive ? "restored" : "archived";

            return Result<object>.Success(existingCategory.Id, $"Category {status} successfully");
        }
    }
}
