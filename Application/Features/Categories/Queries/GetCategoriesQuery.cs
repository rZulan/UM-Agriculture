using Application.DTO.Category;
using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public record GetCategoriesQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetCategoryDTO>>>;
    public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoriesQuery, GetAllResult<List<GetCategoryDTO>>>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        public async Task<GetAllResult<List<GetCategoryDTO>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = categories.Select(x => new GetCategoryDTO
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
                    TotalCount = await _categoryRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
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

            return GetAllResult<List<GetCategoryDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
