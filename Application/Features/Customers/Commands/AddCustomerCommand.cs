using Application.DTO.Customer;
using Application.Interfaces;
using Application.Results;
using Domain.Entities.Masterlist;
using MediatR;
using System.Net;

namespace Application.Features.Customers.Commands
{
    /// <summary>Command to create a new customer.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="AddCustomerDTO">The customer data to be created.</param>
    public record AddCustomerCommand(int? UserId, AddCustomerDTO AddCustomerDTO) : IRequest<Result<object>>;
    public class AddCustomerCommandHandler(ICustomerRepository customerRepository, IUserRepository userRepository) : IRequestHandler<AddCustomerCommand, Result<object>>
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
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

            var existingCustomer = await _customerRepository.GetByNameAsync(request.AddCustomerDTO.Name, cancellationToken);

            if (existingCustomer != null)
            {
                return Result<object>.Failure("Customer already exists", HttpStatusCode.Conflict);
            }

            var customer = new Customer
            {
                Name = request.AddCustomerDTO.Name,
                CreatedAt = DateTime.UtcNow,
                CreatedById = existingUser.Id
            };

            await _customerRepository.AddAsync(customer, cancellationToken);

            return Result<object>.Success(customer.Id, "Customer created successfully", HttpStatusCode.Created);
        }
    }
}
