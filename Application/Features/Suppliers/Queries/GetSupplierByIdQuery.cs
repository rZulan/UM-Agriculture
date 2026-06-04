using Application.DTO.Supplier;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Suppliers.Queries
{
    public record GetSupplierByIdQuery(int Id) : IRequest<Result<GetSupplierDTO>>;
    public class GetSupplierByIdQueryHandler(ISupplierRepository supplierRepository) : IRequestHandler<GetSupplierByIdQuery, Result<GetSupplierDTO>>
    {
        private readonly ISupplierRepository _supplierRepository = supplierRepository;

        public async Task<Result<GetSupplierDTO>> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierRepository.GetByIdAsync(request.Id, cancellationToken);

            if (supplier == null)
            {
                return Result<GetSupplierDTO>.Failure("Supplier not found", HttpStatusCode.NotFound);
            }

            var result = new GetSupplierDTO
            {
                Id = supplier.Id,
                IsActive = supplier.IsActive,
                Name = supplier.Name
            };

            return Result<GetSupplierDTO>.Success(result);
        }
    }
}
