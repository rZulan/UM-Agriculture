using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Departments.Commands
{
    /// <summary>Command to activate or deactivate a department.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the department to toggle.</param>
    /// <param name="IsActive">The desired active state.</param>
    public record ToggleDepartmentActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleDepartmentActiveCommandHandler(IDepartmentRepository departmentRepository, IUserRepository userRepository) : IRequestHandler<ToggleDepartmentActiveCommand, Result<object>>
    {
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleDepartmentActiveCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == null)
            {
                return Result<object>.Failure("User is not signed in", HttpStatusCode.Unauthorized);
            }

            var existingUser = await _userRepository.GetByIdAsync(request.UserId.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingDepartment = await _departmentRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingDepartment == null)
            {
                return Result<object>.Failure("Department not found");
            }

            if (request.IsActive && existingDepartment.IsActive)
            {
                return Result<object>.Failure("Department is already active");
            }

            if (!request.IsActive && !existingDepartment.IsActive)
            {
                return Result<object>.Failure("Department is already archived");
            }

            existingDepartment.IsActive = request.IsActive;
            existingDepartment.UpdatedAt = DateTime.UtcNow;
            existingDepartment.UpdatedById = existingUser.Id;

            await _departmentRepository.UpdateAsync(existingDepartment, cancellationToken);

            var status = existingDepartment.IsActive ? "restored" : "archived";

            return Result<object>.Success(existingDepartment.Id, $"Department {status} successfully");
        }
    }
}
