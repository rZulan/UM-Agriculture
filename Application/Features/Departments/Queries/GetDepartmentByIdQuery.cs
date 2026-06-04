using Application.DTO.Department;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Departments.Queries
{
    public record GetDepartmentByIdQuery(int Id) : IRequest<Result<GetDepartmentDTO>>;
    public class GetDepartmentByIdQueryHandler(IDepartmentRepository departmentRepository) : IRequestHandler<GetDepartmentByIdQuery, Result<GetDepartmentDTO>>
    {
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;

        public async Task<Result<GetDepartmentDTO>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByIdAsync(request.Id, cancellationToken);

            if (department == null)
            {
                return Result<GetDepartmentDTO>.Failure("Department not found", HttpStatusCode.NotFound);
            }

            var result = new GetDepartmentDTO
            {
                Id = department.Id,
                IsActive = department.IsActive,
                Name = department.Name
            };

            return Result<GetDepartmentDTO>.Success(result);
        }
    }
}
