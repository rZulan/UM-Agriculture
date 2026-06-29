using Application.DTO.Category;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Categories.Commands
{
    /// <summary>Command to update an existing category.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the category to update.</param>
    /// <param name="UpdateCategoryDTO">The updated category data.</param>
    public record UpdateCategoryCommand(int? UserId, int Id, UpdateCategoryDTO UpdateCategoryDTO) : IRequest<Result<object>>;
    public class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUserRepository userRepository) : IRequestHandler<UpdateCategoryCommand, Result<object>>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
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
                return Result<object>.Failure("Category not found", System.Net.HttpStatusCode.NotFound);
            }

            if (!string.IsNullOrEmpty(request.UpdateCategoryDTO.Name))
            {
                var existingName = await _categoryRepository.CheckDuplicateAsync(request.Id, request.UpdateCategoryDTO.Name, cancellationToken);

                if (existingName)
                {
                    return Result<object>.Failure("Category name already exists", System.Net.HttpStatusCode.BadRequest);
                }

                existingCategory.Name = request.UpdateCategoryDTO.Name;
            }

            existingCategory.UpdatedAt = DateTime.UtcNow;
            existingCategory.UpdatedById = existingUser.Id;

            await _categoryRepository.UpdateAsync(existingCategory, cancellationToken);

            return Result<object>.Success(null, "Category updated successfully");
        }
    }
}
