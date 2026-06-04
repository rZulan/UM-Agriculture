using Application.DTO.QcSection;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcSections.Queries
{
    public record GetQcSectionByIdQuery(int Id) : IRequest<Result<GetQcSectionDTO>>;
    public class GetQcSectionByIdQueryHandler(IQcSectionRepository qcSectionRepository) : IRequestHandler<GetQcSectionByIdQuery, Result<GetQcSectionDTO>>
    {
        private readonly IQcSectionRepository _qcSectionRepository = qcSectionRepository;

        public async Task<Result<GetQcSectionDTO>> Handle(GetQcSectionByIdQuery request, CancellationToken cancellationToken)
        {
            var qcSection = await _qcSectionRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcSection == null)
            {
                return Result<GetQcSectionDTO>.Failure("QcSection not found", HttpStatusCode.NotFound);
            }

            var result = new GetQcSectionDTO
            {
                Id = qcSection.Id,
                IsActive = qcSection.IsActive,
                Name = qcSection.Name,
                Description = qcSection.Description ?? "N/A",
                Order = qcSection.Order,
                QcFormId = qcSection.QcFormId,
            };

            return Result<GetQcSectionDTO>.Success(result);
        }
    }
}
