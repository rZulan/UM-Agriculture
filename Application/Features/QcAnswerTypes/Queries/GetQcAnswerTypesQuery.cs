using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.QcAnswerType;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.QcAnswerTypes.Queries
{
    /// <summary>Query to retrieve a filtered, sorted, and paginated list of QC answer types.</summary>
    /// <param name="GenericFiltersDTO">Search and pagination filters.</param>
    /// <param name="Sort">Sort direction and field.</param>
    public record GetQcAnswerTypesQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetQcAnswerTypeDTO>>>;
    public class GetQcAnswerTypesQueryHandler(IQcAnswerTypeRepository qcAnswerTypeRepository) : IRequestHandler<GetQcAnswerTypesQuery, GetAllResult<List<GetQcAnswerTypeDTO>>>
    {
        private readonly IQcAnswerTypeRepository _qcAnswerTypeRepository = qcAnswerTypeRepository;

        public async Task<GetAllResult<List<GetQcAnswerTypeDTO>>> Handle(GetQcAnswerTypesQuery request, CancellationToken cancellationToken)
        {
            var qcAnswerTypes = await _qcAnswerTypeRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = qcAnswerTypes.Select(x => new GetQcAnswerTypeDTO
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
                    TotalCount = await _qcAnswerTypeRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
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

            return GetAllResult<List<GetQcAnswerTypeDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
