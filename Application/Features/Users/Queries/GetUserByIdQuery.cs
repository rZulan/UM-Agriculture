using Application.DTO.User;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Users.Queries
{
    public record GetUserByIdQuery(int Id) : IRequest<Result<GetUserDTO>>;
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<GetUserDTO>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<GetUserDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

            if (user is null)
            {
                return Result<GetUserDTO>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var userDto = new GetUserDTO
            {
                Id = user.Id,
                IsActive = user.IsActive,
                Username = user.Username,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName ?? "N/A",
                LastName = user.LastName,
                Suffix = user.Suffix ?? "N/A",
                IDPrefix = user.IDPrefix,
                IDNumber = user.IDNumber,
                Role = user.UserRoles.FirstOrDefault()?.Role?.Name ?? "N/A",
                Permissions = user.UserRoles
                    .SelectMany(x => x.Role!.RolePermissions.Select(rp => rp.Permission!.Name))
                    .Distinct()
                    .ToList()
            };

            return Result<GetUserDTO>.Success(userDto);
        }
    }
}
