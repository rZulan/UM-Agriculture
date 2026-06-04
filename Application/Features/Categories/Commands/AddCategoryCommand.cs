using Application.DTO.Category;
using Application.Interfaces;
using Application.Results;
using Domain.Entities.Masterlist;
using MediatR;
using System.Net;

namespace Application.Features.Categories.Commands
{
    public record AddCategoryCommand(int? UserId, AddCategoryDTO AddCategoryDTO) : IRequest<Result<object>>;
    public class AddCategoryCommandHandler(ICategoryRepository categoryRepository, IUserRepository userRepository) : IRequestHandler<AddCategoryCommand, Result<object>>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingCategory = await _categoryRepository.GetByNameAsync(request.AddCategoryDTO.Name, cancellationToken);

            if (existingCategory != null)
            {
                return Result<object>.Failure("Category already exists", HttpStatusCode.Conflict);
            }

            var category = new Category
            {
                Name = request.AddCategoryDTO.Name,
                CreatedAt = DateTime.UtcNow,
                CreatedById = existingUser.Id
            };

            await _categoryRepository.AddAsync(category, cancellationToken);

            return Result<object>.Success(category.Id, "Category created successfully", HttpStatusCode.Created);
        }
    }
}
