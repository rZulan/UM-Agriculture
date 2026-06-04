using Application.DTO.QcResponse;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcResponses.Queries
{
    public record GetQcResponseByIdQuery(int Id) : IRequest<Result<object>>;
    public class GetQcResponseByIdQueryHandler(IQcResponseRepository qcResponseRepository) : IRequestHandler<GetQcResponseByIdQuery, Result<object>>
    {
        private readonly IQcResponseRepository _qcResponseRepository = qcResponseRepository;

        public async Task<Result<object>> Handle(GetQcResponseByIdQuery request, CancellationToken cancellationToken)
        {
            var qcResponse = await _qcResponseRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcResponse == null)
            {
                return Result<object>.Failure("QcResponse not found", HttpStatusCode.NotFound);
            }

            if (qcResponse.Dispatch != null)
            {
                var result = new GetDispatchQcResponseDTO
                {
                    Id = qcResponse.Id,
                    IsActive = qcResponse.IsActive,
                    FormName = qcResponse.QcForm.QcCategory.Name + " - " + qcResponse.QcForm.QcType.Name + " Form",
                    DispatchId = qcResponse.DispatchId,
                    BatchNumber = qcResponse.Dispatch.BatchNumber,
                    ItemCode = qcResponse.Dispatch.Product.ItemCode,
                    Description = qcResponse.Dispatch.Product.Description,
                    Uom = qcResponse.Dispatch.Product.Uom.ShortName,
                    QuantityOut = qcResponse.Dispatch.QuantityOut,
                    QuantityReturn = qcResponse.Dispatch.QuantityReturn,
                    HarvestDate = qcResponse.Dispatch.HarvestDate,
                };

                return Result<object>.Success(result);
            }

            return Result<object>.Failure("Invalid QcResponse data", HttpStatusCode.BadRequest);
        }
    }
}
