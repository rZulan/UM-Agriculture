using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.Product;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.Products.Queries
{
    public record GetProductsQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetProductDTO>>>;
    public class GetProductsQueryHandler(IProductRepository productRepository) : IRequestHandler<GetProductsQuery, GetAllResult<List<GetProductDTO>>>
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<GetAllResult<List<GetProductDTO>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = products.Select(x => new GetProductDTO
            {
                Id = x.Id,
                IsActive = x.IsActive,
                ItemCode = x.ItemCode,
                Description = x.Description,
                ProductCategory = x.ProductCategory.Name,
                Uom = x.Uom.ShortName + " - " + x.Uom.Name
            }).ToList();

            PaginationInfo? paginationInfo = null;

            if (request.GenericFiltersDTO.UsePagination == true)
            {
                paginationInfo = new PaginationInfo
                {
                    CurrentPage = request.GenericFiltersDTO.PageNumber,
                    PageSize = request.GenericFiltersDTO.PageSize,
                    TotalCount = await _productRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
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

            return GetAllResult<List<GetProductDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
