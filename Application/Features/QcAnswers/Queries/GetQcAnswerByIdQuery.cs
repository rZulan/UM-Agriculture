using Application.DTO.QcAnswer;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcAnswers.Queries
{
    public record GetQcAnswerByIdQuery(int Id) : IRequest<Result<GetQcAnswerDTO>>;
    public class GetQcAnswerByIdQueryHandler(IQcAnswerRepository qcAnswerRepository) : IRequestHandler<GetQcAnswerByIdQuery, Result<GetQcAnswerDTO>>
    {
        private readonly IQcAnswerRepository _qcAnswerRepository = qcAnswerRepository;

        public async Task<Result<GetQcAnswerDTO>> Handle(GetQcAnswerByIdQuery request, CancellationToken cancellationToken)
        {
            var qcAnswer = await _qcAnswerRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcAnswer == null)
            {
                return Result<GetQcAnswerDTO>.Failure("QcAnswer not found", HttpStatusCode.NotFound);
            }

            var result = new GetQcAnswerDTO
            {
                Id = qcAnswer.Id,
                IsActive = qcAnswer.IsActive,
                Answer = qcAnswer.Answer,
                QcResponseId = qcAnswer.QcResponseId,
                QcQuestionId = qcAnswer.QcQuestionId,
            };

            return Result<GetQcAnswerDTO>.Success(result);
        }
    }
}
