using Application.DTO.Dispatch;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Dispatches.Queries
{
    public record GetDispatchByIdQuery(int Id) : IRequest<Result<GetDispatchDTO>>;
    public class GetDispatchByIdQueryHandler : IRequestHandler<GetDispatchByIdQuery, Result<GetDispatchDTO>>
    {
        private readonly IDispatchRepository _dispatchRepository;

        public GetDispatchByIdQueryHandler(IDispatchRepository dispatchRepository)
        {
            _dispatchRepository = dispatchRepository;
        }

        public async Task<Result<GetDispatchDTO>> Handle(GetDispatchByIdQuery request, CancellationToken cancellationToken)
        {
            var dispatch = await _dispatchRepository.GetByIdAsync(request.Id, cancellationToken);

            if (dispatch == null)
            {
                return Result<GetDispatchDTO>.Failure("Dispatch not found", HttpStatusCode.NotFound);
            }

            var result = new GetDispatchDTO
            {
                Id = dispatch.Id,
                IsActive = dispatch.IsActive,
                BatchNumber = dispatch.BatchNumber,
                ItemCode = dispatch.Product.ItemCode,
                Description = dispatch.Product.Description,
                Uom = dispatch.Product.Uom.ShortName,
                QuantityOut = dispatch.QuantityOut,
                QuantityReturn = dispatch.QuantityReturn,
                HarvestDate = dispatch.HarvestDate
            };

            return Result<GetDispatchDTO>.Success(result);
        }
    }
}