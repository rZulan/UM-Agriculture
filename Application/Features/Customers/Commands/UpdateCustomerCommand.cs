using Application.DTO.Customer;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Customers.Commands
{
    /// <summary>Command to update an existing customer.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the customer to update.</param>
    /// <param name="UpdateCustomerDTO">The updated customer data.</param>
    public record UpdateCustomerCommand(int? UserId, int Id, UpdateCustomerDTO UpdateCustomerDTO) : IRequest<Result<object>>;
    public class UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IUserRepository userRepository) : IRequestHandler<UpdateCustomerCommand, Result<object>>
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
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
                return Result<object>.Failure("Customer not found", System.Net.HttpStatusCode.NotFound);
            }

            if (!string.IsNullOrEmpty(request.UpdateCustomerDTO.Name))
            {
                var existingName = await _customerRepository.CheckDuplicateAsync(request.Id, request.UpdateCustomerDTO.Name, cancellationToken);

                if (existingName)
                {
                    return Result<object>.Failure("Customer name already exists", System.Net.HttpStatusCode.BadRequest);
                }

                existingCustomer.Name = request.UpdateCustomerDTO.Name;
            }

            existingCustomer.UpdatedAt = DateTime.UtcNow;
            existingCustomer.UpdatedById = existingUser.Id;

            await _customerRepository.UpdateAsync(existingCustomer, cancellationToken);

            return Result<object>.Success(null, "Customer updated successfully");
        }
    }
}
