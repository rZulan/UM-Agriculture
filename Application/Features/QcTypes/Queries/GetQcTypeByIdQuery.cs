using Application.DTO.QcType;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcTypes.Queries
{
    /// <summary>Query to retrieve a single QC type by its ID.</summary>
    /// <param name="Id">The unique identifier of the QC type to retrieve.</param>
    public record GetQcTypeByIdQuery(int Id) : IRequest<Result<GetQcTypeDTO>>;
    public class GetQcTypeByIdQueryHandler(IQcTypeRepository qcTypeRepository) : IRequestHandler<GetQcTypeByIdQuery, Result<GetQcTypeDTO>>
    {
        private readonly IQcTypeRepository _qcTypeRepository = qcTypeRepository;

        public async Task<Result<GetQcTypeDTO>> Handle(GetQcTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var qcType = await _qcTypeRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcType == null)
            {
                return Result<GetQcTypeDTO>.Failure("QcType not found", HttpStatusCode.NotFound);
            }

            var result = new GetQcTypeDTO
            {
                Id = qcType.Id,
                IsActive = qcType.IsActive,
                Name = qcType.Name,
            };

            return Result<GetQcTypeDTO>.Success(result);
        }
    }
}
