using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.QcSection;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.QcSections.Queries
{
    /// <summary>Query to retrieve a filtered, sorted, and paginated list of QC sections.</summary>
    /// <param name="GenericFiltersDTO">Search and pagination filters.</param>
    /// <param name="Sort">Sort direction and field.</param>
    public record GetQcSectionsQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetQcSectionDTO>>>;
    public class GetQcSectionsQueryHandler(IQcSectionRepository qcSectionRepository) : IRequestHandler<GetQcSectionsQuery, GetAllResult<List<GetQcSectionDTO>>>
    {
        private readonly IQcSectionRepository _qcSectionRepository = qcSectionRepository;

        public async Task<GetAllResult<List<GetQcSectionDTO>>> Handle(GetQcSectionsQuery request, CancellationToken cancellationToken)
        {
            var qcSections = await _qcSectionRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = qcSections.Select(x => new GetQcSectionDTO
            {
                Id = x.Id,
                IsActive = x.IsActive,
                Name = x.Name,
                Description = x.Description ?? "N/A",
                Order = x.Order,
                QcFormId = x.QcFormId,
            }).ToList();

            PaginationInfo? paginationInfo = null;

            if (request.GenericFiltersDTO.UsePagination == true)
            {
                paginationInfo = new PaginationInfo
                {
                    CurrentPage = request.GenericFiltersDTO.PageNumber,
                    PageSize = request.GenericFiltersDTO.PageSize,
                    TotalCount = await _qcSectionRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
                };
            }

            var sortInfo = new SortInfo
            {
                SortColumns = ["id", "order", "qcFormId"],
                CurrentSort = request.Sort != null ? new CurrentSort
                {
                    Column = request.Sort.SortBy,
                    Direction = request.Sort.SortDirection
                } : null
            };

            return GetAllResult<List<GetQcSectionDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
