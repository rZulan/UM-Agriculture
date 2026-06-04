using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Departments.Commands
{
    public record ToggleDepartmentActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleDepartmentActiveCommandHandler(IDepartmentRepository departmentRepository, IUserRepository userRepository) : IRequestHandler<ToggleDepartmentActiveCommand, Result<object>>
    {
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleDepartmentActiveCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

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
