using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Permissions.Commands
{
    public record TogglePermissionActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class TogglePermissionActiveCommandHandler(IPermissionRepository permissionRepository, IUserRepository userRepository) : IRequestHandler<TogglePermissionActiveCommand, Result<object>>
    {
        private readonly IPermissionRepository _permissionRepository = permissionRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(TogglePermissionActiveCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingPermission = await _permissionRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingPermission == null)
            {
                return Result<object>.Failure("Permission not found");
            }

            if (request.IsActive && existingPermission.IsActive)
            {
                return Result<object>.Failure("Permission is already active");
            }

            if (!request.IsActive && !existingPermission.IsActive)
            {
                return Result<object>.Failure("Permission is already archived");
            }

            existingPermission.IsActive = request.IsActive;
            existingPermission.UpdatedAt = DateTime.UtcNow;
            existingPermission.UpdatedById = existingUser.Id;

            await _permissionRepository.UpdateAsync(existingPermission, cancellationToken);

            var status = existingPermission.IsActive ? "restored" : "archived";

            return Result<object>.Success(existingPermission.Id, $"Permission {status} successfully");
        }
    }
}
