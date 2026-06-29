using Application.DTO.Category;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Categories.Queries
{
    /// <summary>Query to retrieve a single category by its ID.</summary>
    /// <param name="Id">The unique identifier of the category to retrieve.</param>
    public record GetCategoryByIdQuery(int Id) : IRequest<Result<GetCategoryDTO>>;
    public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryByIdQuery, Result<GetCategoryDTO>>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        public async Task<Result<GetCategoryDTO>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (category == null)
            {
                return Result<GetCategoryDTO>.Failure("Category not found", HttpStatusCode.NotFound);
            }

            var result = new GetCategoryDTO
            {
                Id = category.Id,
                IsActive = category.IsActive,
                Name = category.Name
            };

            return Result<GetCategoryDTO>.Success(result);
        }
    }
}
