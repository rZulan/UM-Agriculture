using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.User;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.Users.Queries
{
    public record GetUsersQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetUserDTO>>>;
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetAllResult<List<GetUserDTO>>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetAllResult<List<GetUserDTO>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = users.Select(u => new GetUserDTO
            {
                Id = u.Id,
                IsActive = u.IsActive,
                Username = u.Username,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName ?? "N/A",
                LastName = u.LastName,
                Suffix = u.Suffix ?? "N/A",
                IDPrefix = u.IDPrefix,
                IDNumber = u.IDNumber,
                Role = u.UserRoles.FirstOrDefault()?.Role?.Name ?? "N/A",
                Permissions = [.. u.UserRoles
                    .SelectMany(ur => ur.Role!.RolePermissions)
                    .Select(rp => rp.Permission!.Name)
                    .Distinct()]
            }).ToList() ?? [];

            PaginationInfo? paginationInfo = null;

            if (request.GenericFiltersDTO.UsePagination == true)
            {
                paginationInfo = new PaginationInfo
                {
                    CurrentPage = request.GenericFiltersDTO.PageNumber,
                    PageSize = request.GenericFiltersDTO.PageSize,
                    TotalCount = await _userRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
                };
            }

            var sortInfo = new SortInfo
            {
                SortColumns = ["id", "username", "firstname", "lastname"],
                CurrentSort = request.Sort != null ? new CurrentSort
                {
                    Column = request.Sort.SortBy,
                    Direction = request.Sort.SortDirection
                } : null
            };

            return GetAllResult<List<GetUserDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
