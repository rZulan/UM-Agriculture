using Application.DTO.QcForm;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcForms.Queries
{
    /// <summary>Query to retrieve a single QC form by its ID.</summary>
    /// <param name="Id">The unique identifier of the QC form to retrieve.</param>
    public record GetQcFormByIdQuery(int Id) : IRequest<Result<GetQcFormDTO>>;
    public class GetQcFormByIdQueryHandler(IQcFormRepository qcFormRepository) : IRequestHandler<GetQcFormByIdQuery, Result<GetQcFormDTO>>
    {
        private readonly IQcFormRepository _qcFormRepository = qcFormRepository;

        public async Task<Result<GetQcFormDTO>> Handle(GetQcFormByIdQuery request, CancellationToken cancellationToken)
        {
            var qcForm = await _qcFormRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcForm == null)
            {
                return Result<GetQcFormDTO>.Failure("QcForm not found", HttpStatusCode.NotFound);
            }

            var result = new GetQcFormDTO
            {
                Id = qcForm.Id,
                IsActive = qcForm.IsActive,
            };

            return Result<GetQcFormDTO>.Success(result);
        }
    }
}
