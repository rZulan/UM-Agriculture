using Application.DTO.Department;
using Application.Interfaces;
using Application.Results;
using Domain.Entities.Masterlist;
using MediatR;
using System.Net;

namespace Application.Features.Departments.Commands
{
    public record AddDepartmentCommand(int? UserId, AddDepartmentDTO AddDepartmentDTO) : IRequest<Result<object>>;
    public class AddDepartmentCommandHandler(IDepartmentRepository departmentRepository, IUserRepository userRepository) : IRequestHandler<AddDepartmentCommand, Result<object>>
    {
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingDepartment = await _departmentRepository.GetByNameAsync(request.AddDepartmentDTO.Name, cancellationToken);

            if (existingDepartment != null)
            {
                return Result<object>.Failure("Department already exists", HttpStatusCode.Conflict);
            }

            var department = new Department
            {
                Name = request.AddDepartmentDTO.Name,
                CreatedAt = DateTime.UtcNow,
                CreatedById = existingUser.Id
            };

            await _departmentRepository.AddAsync(department, cancellationToken);

            return Result<object>.Success(department.Id, "Department created successfully", HttpStatusCode.Created);
        }
    }
}
