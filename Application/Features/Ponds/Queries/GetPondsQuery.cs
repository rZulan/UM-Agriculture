using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.Pond;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.Ponds.Queries
{
    public record GetPondsQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetPondDTO>>>;
    public class GetPondsQueryHandler(IPondRepository pondRepository) : IRequestHandler<GetPondsQuery, GetAllResult<List<GetPondDTO>>>
    {
        private readonly IPondRepository _pondRepository = pondRepository;

        public async Task<GetAllResult<List<GetPondDTO>>> Handle(GetPondsQuery request, CancellationToken cancellationToken)
        {
            var ponds = await _pondRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = ponds.Select(x => new GetPondDTO
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
                    TotalCount = await _pondRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
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

            return GetAllResult<List<GetPondDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
