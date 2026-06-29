using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.ProductCategory;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.ProductCategories.Queries
{
    /// <summary>Query to retrieve a filtered, sorted, and paginated list of product categories.</summary>
    /// <param name="GenericFiltersDTO">Search and pagination filters.</param>
    /// <param name="Sort">Sort direction and field.</param>
    public record GetProductCategoriesQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetProductCategoryDTO>>>;
    public class GetProductCategoriesQueryHandler(IProductCategoryRepository productCategoryRepository) : IRequestHandler<GetProductCategoriesQuery, GetAllResult<List<GetProductCategoryDTO>>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository = productCategoryRepository;

        public async Task<GetAllResult<List<GetProductCategoryDTO>>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var productCategorys = await _productCategoryRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = productCategorys.Select(x => new GetProductCategoryDTO
            {
                Id = x.Id,
                IsActive = x.IsActive,
                Name = x.Name,
            }).ToList();

            PaginationInfo? paginationInfo = null;

            if (request.GenericFiltersDTO.UsePagination == true)
            {
                paginationInfo = new PaginationInfo
                {
                    CurrentPage = request.GenericFiltersDTO.PageNumber,
                    PageSize = request.GenericFiltersDTO.PageSize,
                    TotalCount = await _productCategoryRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
                };
            }

            var sortInfo = new SortInfo
            {
                SortColumns = ["id", "name"],
                CurrentSort = request.Sort != null ? new CurrentSort
                {
                    Column = request.Sort.SortBy,
                    Direction = request.Sort.SortDirection
                } : null
            };

            return GetAllResult<List<GetProductCategoryDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
