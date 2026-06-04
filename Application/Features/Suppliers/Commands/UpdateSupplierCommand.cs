using Application.DTO.Supplier;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Suppliers.Commands
{
    public record UpdateSupplierCommand(int? UserId, int Id, UpdateSupplierDTO UpdateSupplierDTO) : IRequest<Result<object>>;
    public class UpdateSupplierCommandHandler(ISupplierRepository supplierRepository, IUserRepository userRepository) : IRequestHandler<UpdateSupplierCommand, Result<object>>
    {
        private readonly ISupplierRepository _supplierRepository = supplierRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingSupplier = await _supplierRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingSupplier == null)
            {
                return Result<object>.Failure("Supplier not found", System.Net.HttpStatusCode.NotFound);
            }

            if (!string.IsNullOrEmpty(request.UpdateSupplierDTO.Name))
            {
                var existingName = await _supplierRepository.CheckDuplicateAsync(request.Id, request.UpdateSupplierDTO.Name, cancellationToken);

                if (existingName)
                {
                    return Result<object>.Failure("Supplier name already exists", System.Net.HttpStatusCode.BadRequest);
                }

                existingSupplier.Name = request.UpdateSupplierDTO.Name;
            }

            existingSupplier.UpdatedAt = DateTime.UtcNow;
            existingSupplier.UpdatedById = existingUser.Id;

            await _supplierRepository.UpdateAsync(existingSupplier, cancellationToken);

            return Result<object>.Success(null, "Supplier updated successfully");
        }
    }
}
