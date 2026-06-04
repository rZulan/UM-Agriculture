using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Roles.Commands
{
    public record ToggleRoleActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleRoleActiveCommandHandler(IRoleRepository roleRepository, IUserRepository userRepository) : IRequestHandler<ToggleRoleActiveCommand, Result<object>>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleRoleActiveCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingRole = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingRole == null)
            {
                return Result<object>.Failure("Role not found");
            }

            if (request.IsActive && existingRole.IsActive)
            {
                return Result<object>.Failure("Role is already active");
            }

            if (!request.IsActive && !existingRole.IsActive)
            {
                return Result<object>.Failure("Role is already archived");
            }

            existingRole.IsActive = request.IsActive;
            existingRole.UpdatedAt = DateTime.UtcNow;
            existingUser.UpdatedById = existingUser.Id;

            await _roleRepository.UpdateAsync(existingRole, cancellationToken);

            var status = existingRole.IsActive ? "restored" : "archived";

            return Result<object>.Success(existingRole.Id, $"Role {status} successfully");
        }
    }
}
