using Application.DTO.Customer;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Customers.Queries
{
    /// <summary>Query to retrieve a single customer by its ID.</summary>
    /// <param name="Id">The unique identifier of the customer to retrieve.</param>
    public record GetCustomerByIdQuery(int Id) : IRequest<Result<GetCustomerDTO>>;
    public class GetCustomerByIdQueryHandler(ICustomerRepository customerRepository) : IRequestHandler<GetCustomerByIdQuery, Result<GetCustomerDTO>>
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;

        public async Task<Result<GetCustomerDTO>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);

            if (customer == null)
            {
                return Result<GetCustomerDTO>.Failure("Customer not found", HttpStatusCode.NotFound);
            }

            var result = new GetCustomerDTO
            {
                Id = customer.Id,
                IsActive = customer.IsActive,
                Name = customer.Name
            };

            return Result<GetCustomerDTO>.Success(result);
        }
    }
}
