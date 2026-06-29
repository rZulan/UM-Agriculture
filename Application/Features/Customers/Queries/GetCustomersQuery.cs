using Application.DTO.Customer;
using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.Customers.Queries
{
    /// <summary>Query to retrieve a filtered, sorted, and paginated list of customers.</summary>
    /// <param name="GenericFiltersDTO">Search and pagination filters.</param>
    /// <param name="Sort">Sort direction and field.</param>
    public record GetCustomersQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetCustomerDTO>>>;
    public class GetCustomersQueryHandler(ICustomerRepository customerRepository) : IRequestHandler<GetCustomersQuery, GetAllResult<List<GetCustomerDTO>>>
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;

        public async Task<GetAllResult<List<GetCustomerDTO>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = customers.Select(x => new GetCustomerDTO
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
                    TotalCount = await _customerRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
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

            return GetAllResult<List<GetCustomerDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
