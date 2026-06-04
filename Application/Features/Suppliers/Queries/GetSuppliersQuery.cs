using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.Supplier;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.Suppliers.Queries
{
    public record GetSuppliersQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetSupplierDTO>>>;
    public class GetSuppliersQueryHandler(ISupplierRepository supplierRepository) : IRequestHandler<GetSuppliersQuery, GetAllResult<List<GetSupplierDTO>>>
    {
        private readonly ISupplierRepository _supplierRepository = supplierRepository;

        public async Task<GetAllResult<List<GetSupplierDTO>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await _supplierRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = suppliers.Select(x => new GetSupplierDTO
            {
                Id = x.Id,
                IsActive = x.IsActive,
                Name = x.Name
            }).ToList();

            PaginationInfo? paginationInfo = null;

            if (request.GenericFiltersDTO.UsePagination == true)
            {
                paginationInfo = new PaginationInfo
                {
                    CurrentPage = request.GenericFiltersDTO.PageNumber,
                    PageSize = request.GenericFiltersDTO.PageSize,
                    TotalCount = await _supplierRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
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

            return GetAllResult<List<GetSupplierDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
