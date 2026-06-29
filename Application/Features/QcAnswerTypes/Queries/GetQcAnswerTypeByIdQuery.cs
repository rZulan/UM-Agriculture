using Application.DTO.QcAnswerType;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcAnswerTypes.Queries
{
    /// <summary>Query to retrieve a single QC answer type by its ID.</summary>
    /// <param name="Id">The unique identifier of the QC answer type to retrieve.</param>
    public record GetQcAnswerTypeByIdQuery(int Id) : IRequest<Result<GetQcAnswerTypeDTO>>;
    public class GetQcAnswerTypeByIdQueryHandler(IQcAnswerTypeRepository qcAnswerTypeRepository) : IRequestHandler<GetQcAnswerTypeByIdQuery, Result<GetQcAnswerTypeDTO>>
    {
        private readonly IQcAnswerTypeRepository _qcAnswerTypeRepository = qcAnswerTypeRepository;

        public async Task<Result<GetQcAnswerTypeDTO>> Handle(GetQcAnswerTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var qcAnswerType = await _qcAnswerTypeRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcAnswerType == null)
            {
                return Result<GetQcAnswerTypeDTO>.Failure("QcAnswerType not found", HttpStatusCode.NotFound);
            }

            var result = new GetQcAnswerTypeDTO
            {
                Id = qcAnswerType.Id,
                IsActive = qcAnswerType.IsActive,
                Name = qcAnswerType.Name,
            };

            return Result<GetQcAnswerTypeDTO>.Success(result);
        }
    }
}
