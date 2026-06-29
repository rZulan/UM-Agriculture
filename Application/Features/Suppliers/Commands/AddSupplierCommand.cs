using Application.DTO.Supplier;
using Application.Interfaces;
using Application.Results;
using Domain.Entities.Masterlist;
using MediatR;
using System.Net;

namespace Application.Features.Suppliers.Commands
{
    /// <summary>Command to create a new supplier.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="AddSupplierDTO">The supplier data to be created.</param>
    public record AddSupplierCommand(int? UserId, AddSupplierDTO AddSupplierDTO) : IRequest<Result<object>>;
    public class AddSupplierCommandHandler(ISupplierRepository supplierRepository, IUserRepository userRepository) : IRequestHandler<AddSupplierCommand, Result<object>>
    {
        private readonly ISupplierRepository _supplierRepository = supplierRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(AddSupplierCommand request, CancellationToken cancellationToken)
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

            var existingSupplier = await _supplierRepository.GetByNameAsync(request.AddSupplierDTO.Name, cancellationToken);

            if (existingSupplier != null)
            {
                return Result<object>.Failure("Supplier already exists", HttpStatusCode.Conflict);
            }

            var supplier = new Supplier
            {
                Name = request.AddSupplierDTO.Name,
                CreatedAt = DateTime.UtcNow,
                CreatedById = existingUser.Id,
            };

            await _supplierRepository.AddAsync(supplier, cancellationToken);

            return Result<object>.Success(supplier.Id, "Supplier created successfully", HttpStatusCode.Created);
        }
    }
}
