using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Customers.Commands
{
    /// <summary>Command to activate or deactivate a customer.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the customer to toggle.</param>
    /// <param name="IsActive">The desired active state.</param>
    public record ToggleCustomerActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleCustomerActiveCommandHandler(ICustomerRepository customerRepository, IUserRepository userRepository) : IRequestHandler<ToggleCustomerActiveCommand, Result<object>>
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleCustomerActiveCommand request, CancellationToken cancellationToken)
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

            var existingCustomer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingCustomer == null)
            {
                return Result<object>.Failure("Customer not found");
            }

            if (request.IsActive && existingCustomer.IsActive)
            {
                return Result<object>.Failure("Customer is already active");
            }

            if (!request.IsActive && !existingCustomer.IsActive)
            {    
                return Result<object>.Failure("Customer is already archived");
            }

            existingCustomer.IsActive = request.IsActive;
            existingCustomer.UpdatedAt = DateTime.UtcNow;
            existingCustomer.UpdatedById = existingUser.Id;

            await _customerRepository.UpdateAsync(existingCustomer, cancellationToken);

            var status = existingCustomer.IsActive ? "restored" : "archived";

            return Result<object>.Success(existingCustomer.Id, $"Customer {status} successfully");
        }
    }
}
