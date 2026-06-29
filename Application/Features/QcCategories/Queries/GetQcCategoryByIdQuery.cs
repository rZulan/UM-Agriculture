using Application.DTO.QcCategory;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcCategories.Queries
{
    /// <summary>Query to retrieve a single QC category by its ID.</summary>
    /// <param name="Id">The unique identifier of the QC category to retrieve.</param>
    public record GetQcCategoryByIdQuery(int Id) : IRequest<Result<GetQcCategoryDTO>>;
    public class GetQcCategoryByIdQueryHandler(IQcCategoryRepository qcCategoryRepository) : IRequestHandler<GetQcCategoryByIdQuery, Result<GetQcCategoryDTO>>
    {
        private readonly IQcCategoryRepository _qcCategoryRepository = qcCategoryRepository;

        public async Task<Result<GetQcCategoryDTO>> Handle(GetQcCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var qcCategory = await _qcCategoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcCategory == null)
            {
                return Result<GetQcCategoryDTO>.Failure("QcCategory not found", HttpStatusCode.NotFound);
            }

            var result = new GetQcCategoryDTO
            {
                Id = qcCategory.Id,
                IsActive = qcCategory.IsActive,
                Name = qcCategory.Name,
            };

            return Result<GetQcCategoryDTO>.Success(result);
        }
    }
}
