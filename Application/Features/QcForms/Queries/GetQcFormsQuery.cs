using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.QcForm;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.QcForms.Queries
{
    public record GetQcFormsQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetQcFormDTO>>>;
    public class GetQcFormsQueryHandler(IQcFormRepository qcFormRepository) : IRequestHandler<GetQcFormsQuery, GetAllResult<List<GetQcFormDTO>>>
    {
        private readonly IQcFormRepository _qcFormRepository = qcFormRepository;

        public async Task<GetAllResult<List<GetQcFormDTO>>> Handle(GetQcFormsQuery request, CancellationToken cancellationToken)
        {
            var qcForms = await _qcFormRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = qcForms.Select(x => new GetQcFormDTO
            {
                Id = x.Id,
                IsActive = x.IsActive,
            }).ToList();

            PaginationInfo? paginationInfo = null;

            if (request.GenericFiltersDTO.UsePagination == true)
            {
                paginationInfo = new PaginationInfo
                {
                    CurrentPage = request.GenericFiltersDTO.PageNumber,
                    PageSize = request.GenericFiltersDTO.PageSize,
                    TotalCount = await _qcFormRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
                };
            }

            var sortInfo = new SortInfo
            {
                SortColumns = ["id", "qcCategoryId"],
                CurrentSort = request.Sort != null ? new CurrentSort
                {
                    Column = request.Sort.SortBy,
                    Direction = request.Sort.SortDirection
                } : null
            };

            return GetAllResult<List<GetQcFormDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
