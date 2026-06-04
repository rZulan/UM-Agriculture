using Application.DTO.QcQuestion;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcQuestions.Queries
{
    public record GetQcQuestionByIdQuery(int Id) : IRequest<Result<GetQcQuestionDTO>>;
    public class GetQcQuestionByIdQueryHandler(IQcQuestionRepository qcQuestionRepository) : IRequestHandler<GetQcQuestionByIdQuery, Result<GetQcQuestionDTO>>
    {
        private readonly IQcQuestionRepository _qcQuestionRepository = qcQuestionRepository;

        public async Task<Result<GetQcQuestionDTO>> Handle(GetQcQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var qcQuestion = await _qcQuestionRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcQuestion == null)
            {
                return Result<GetQcQuestionDTO>.Failure("QcQuestion not found", HttpStatusCode.NotFound);
            }

            var result = new GetQcQuestionDTO
            {
                Id = qcQuestion.Id,
                IsActive = qcQuestion.IsActive,
                Question = qcQuestion.Question,
                IsRequired = qcQuestion.IsRequired,
                CorrectAnswer = qcQuestion.CorrectAnswer,
                Order = qcQuestion.Order,
                QcSectionId = qcQuestion.QcSectionId,
                QcAnswerTypeId = qcQuestion.QcAnswerTypeId,
            };

            return Result<GetQcQuestionDTO>.Success(result);
        }
    }
}
