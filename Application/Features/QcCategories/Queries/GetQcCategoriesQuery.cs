using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.QcCategory;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.QcCategories.Queries
{
    public record GetQcCategoriesQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetQcCategoryDTO>>>;
    public class GetQcCategoriesQueryHandler(IQcCategoryRepository qcCategoryRepository) : IRequestHandler<GetQcCategoriesQuery, GetAllResult<List<GetQcCategoryDTO>>>
    {
        private readonly IQcCategoryRepository _qcCategoryRepository = qcCategoryRepository;

        public async Task<GetAllResult<List<GetQcCategoryDTO>>> Handle(GetQcCategoriesQuery request, CancellationToken cancellationToken)
        {
            var qcCategories = await _qcCategoryRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = qcCategories.Select(x => new GetQcCategoryDTO
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
                    TotalCount = await _qcCategoryRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
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

            return GetAllResult<List<GetQcCategoryDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
