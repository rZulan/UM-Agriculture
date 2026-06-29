using Application.DTO.Farm;
using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.Farms.Queries
{
    /// <summary>Query to retrieve a filtered, sorted, and paginated list of farms.</summary>
    /// <param name="GenericFiltersDTO">Search and pagination filters.</param>
    /// <param name="Sort">Sort direction and field.</param>
    public record GetFarmsQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetFarmDTO>>>;
    public class GetFarmsQueryHandler(IFarmRepository farmRepository) : IRequestHandler<GetFarmsQuery, GetAllResult<List<GetFarmDTO>>>
    {
        private readonly IFarmRepository _farmRepository = farmRepository;

        public async Task<GetAllResult<List<GetFarmDTO>>> Handle(GetFarmsQuery request, CancellationToken cancellationToken)
        {
            var farms = await _farmRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = farms.Select(x => new GetFarmDTO
            {
                Id = x.Id,
                IsActive = x.IsActive,
                Name = x.Name,
                Address = x.Address
            }).ToList();

            PaginationInfo? paginationInfo = null;

            if (request.GenericFiltersDTO.UsePagination == true)
            {
                paginationInfo = new PaginationInfo
                {
                    CurrentPage = request.GenericFiltersDTO.PageNumber,
                    PageSize = request.GenericFiltersDTO.PageSize,
                    TotalCount = await _farmRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
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

            return GetAllResult<List<GetFarmDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
