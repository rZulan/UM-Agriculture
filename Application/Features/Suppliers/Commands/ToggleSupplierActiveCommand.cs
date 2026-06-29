using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Suppliers.Commands
{
    /// <summary>Command to activate or deactivate a supplier.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the supplier to toggle.</param>
    /// <param name="IsActive">The desired active state.</param>
    public record ToggleSupplierActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleSupplierActiveCommandHandler(ISupplierRepository supplierRepository, IUserRepository userRepository) : IRequestHandler<ToggleSupplierActiveCommand, Result<object>>
    {
        private readonly ISupplierRepository _supplierRepository = supplierRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleSupplierActiveCommand request, CancellationToken cancellationToken)
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

            var existingSupplier = await _supplierRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingSupplier == null)
            {
                return Result<object>.Failure("Supplier not found");
            }

            if (request.IsActive && existingSupplier.IsActive)
            {
                return Result<object>.Failure("Supplier is already active");
            }

            if (!request.IsActive && !existingSupplier.IsActive)
            {
                return Result<object>.Failure("Supplier is already archived");
            }

            existingSupplier.IsActive = request.IsActive;
            existingSupplier.UpdatedAt = DateTime.UtcNow;
            existingSupplier.UpdatedById = existingUser.Id;

            await _supplierRepository.UpdateAsync(existingSupplier, cancellationToken);

            var status = existingSupplier.IsActive ? "restored" : "archived";

            return Result<object>.Success(existingSupplier.Id, $"Supplier {status} successfully");
        }
    }
}
