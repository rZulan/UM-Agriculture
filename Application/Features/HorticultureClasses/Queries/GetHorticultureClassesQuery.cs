using Application.DTO.HorticultureClass;
using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.HorticultureClasses.Queries
{
    /// <summary>Query to retrieve a filtered, sorted, and paginated list of horticulture classes.</summary>
    /// <param name="GenericFiltersDTO">Search and pagination filters.</param>
    /// <param name="Sort">Sort direction and field.</param>
    public record GetHorticultureClassesQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetHorticultureClassDTO>>>;
    public class GetHorticultureClassesQueryHandler(IHorticultureClassRepository horticultureClassRepository) : IRequestHandler<GetHorticultureClassesQuery, GetAllResult<List<GetHorticultureClassDTO>>>
    {
        private readonly IHorticultureClassRepository _horticultureClassRepository = horticultureClassRepository;

        public async Task<GetAllResult<List<GetHorticultureClassDTO>>> Handle(GetHorticultureClassesQuery request, CancellationToken cancellationToken)
        {
            var horticultureClasses = await _horticultureClassRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = horticultureClasses.Select(x => new GetHorticultureClassDTO
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
                    TotalCount = await _horticultureClassRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
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

            return GetAllResult<List<GetHorticultureClassDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
