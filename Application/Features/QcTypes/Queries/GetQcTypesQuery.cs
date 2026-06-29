using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.QcType;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.QcTypes.Queries
{
    /// <summary>Query to retrieve a filtered, sorted, and paginated list of QC types.</summary>
    /// <param name="GenericFiltersDTO">Search and pagination filters.</param>
    /// <param name="Sort">Sort direction and field.</param>
    public record GetQcTypesQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetQcTypeDTO>>>;
    public class GetQcTypesQueryHandler(IQcTypeRepository qcTypeRepository) : IRequestHandler<GetQcTypesQuery, GetAllResult<List<GetQcTypeDTO>>>
    {
        private readonly IQcTypeRepository _qcTypeRepository = qcTypeRepository;

        public async Task<GetAllResult<List<GetQcTypeDTO>>> Handle(GetQcTypesQuery request, CancellationToken cancellationToken)
        {
            var qcTypes = await _qcTypeRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = qcTypes.Select(x => new GetQcTypeDTO
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
                    TotalCount = await _qcTypeRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
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

            return GetAllResult<List<GetQcTypeDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
