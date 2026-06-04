using Application.DTO.Department;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Departments.Commands
{
    public record UpdateDepartmentCommand(int? UserId, int Id, UpdateDepartmentDTO UpdateDepartmentDTO) : IRequest<Result<object>>;
    public class UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository, IUserRepository userRepository) : IRequestHandler<UpdateDepartmentCommand, Result<object>>
    {
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingDepartment = await _departmentRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingDepartment == null)
            {
                return Result<object>.Failure("Department not found", System.Net.HttpStatusCode.NotFound);
            }

            if (!string.IsNullOrEmpty(request.UpdateDepartmentDTO.Name))
            {
                var existingName = await _departmentRepository.CheckDuplicateAsync(request.Id, request.UpdateDepartmentDTO.Name, cancellationToken);

                if (existingName)
                {
                    return Result<object>.Failure("Department name already exists", System.Net.HttpStatusCode.BadRequest);
                }

                existingDepartment.Name = request.UpdateDepartmentDTO.Name;
            }

            existingDepartment.UpdatedAt = DateTime.UtcNow;
            existingDepartment.UpdatedById = existingUser.Id;

            await _departmentRepository.UpdateAsync(existingDepartment, cancellationToken);

            return Result<object>.Success(null, "Department updated successfully");
        }
    }
}
