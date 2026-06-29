using Application.DTO.Department;
using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.Departments.Queries
{
    /// <summary>Query to retrieve a filtered, sorted, and paginated list of departments.</summary>
    /// <param name="GenericFiltersDTO">Search and pagination filters.</param>
    /// <param name="Sort">Sort direction and field.</param>
    public record GetDepartmentsQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetDepartmentDTO>>>;
    public class GetDepartmentsQueryHandler(IDepartmentRepository departmentRepository) : IRequestHandler<GetDepartmentsQuery, GetAllResult<List<GetDepartmentDTO>>>
    {
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;

        public async Task<GetAllResult<List<GetDepartmentDTO>>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var departments = await _departmentRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = departments.Select(x => new GetDepartmentDTO
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
                    TotalCount = await _departmentRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
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

            return GetAllResult<List<GetDepartmentDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
