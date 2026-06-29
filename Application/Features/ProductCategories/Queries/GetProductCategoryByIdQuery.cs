using Application.DTO.ProductCategory;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.ProductCategories.Queries
{
    /// <summary>Query to retrieve a single product category by its ID.</summary>
    /// <param name="Id">The unique identifier of the product category to retrieve.</param>
    public record GetProductCategoryByIdQuery(int Id) : IRequest<Result<GetProductCategoryDTO>>;
    public class GetProductCategoryByIdQueryHandler(IProductCategoryRepository productCategoryRepository) : IRequestHandler<GetProductCategoryByIdQuery, Result<GetProductCategoryDTO>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository = productCategoryRepository;

        public async Task<Result<GetProductCategoryDTO>> Handle(GetProductCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var productCategory = await _productCategoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (productCategory == null)
            {
                return Result<GetProductCategoryDTO>.Failure("ProductCategory not found", HttpStatusCode.NotFound);
            }

            var result = new GetProductCategoryDTO
            {
                Id = productCategory.Id,
                IsActive = productCategory.IsActive,
                Name = productCategory.Name,
            };

            return Result<GetProductCategoryDTO>.Success(result);
        }
    }
}