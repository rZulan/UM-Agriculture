using Application.DTO.QcQuestion;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcQuestions.Commands
{
    /// <summary>Command to update an existing QC question.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the QC question to update.</param>
    /// <param name="UpdateQcQuestionDTO">The updated QC question data.</param>
    public record UpdateQcQuestionCommand(int? UserId, int Id, UpdateQcQuestionDTO UpdateQcQuestionDTO) : IRequest<Result<object>>;
    public class UpdateQcQuestionCommandHandler(IQcQuestionRepository qcQuestionRepository, IQcSectionRepository qcSectionRepository, IQcAnswerTypeRepository qcAnswerTypeRepository, IUserRepository userRepository) : IRequestHandler<UpdateQcQuestionCommand, Result<object>>
    {
        private readonly IQcQuestionRepository _qcQuestionRepository = qcQuestionRepository;
        private readonly IQcSectionRepository _qcSectionRepository = qcSectionRepository;
        private readonly IQcAnswerTypeRepository _qcAnswerTypeRepository = qcAnswerTypeRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdateQcQuestionCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == null)
            {
                return Result<object>.Failure("User is not signed in", HttpStatusCode.Unauthorized);
            }

            var existingUser = await _userRepository.GetByIdAsync(request.UserId.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var qcQuestion = await _qcQuestionRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcQuestion == null)
            {
                return Result<object>.Failure("QcQuestion not found", HttpStatusCode.NotFound);
            }

            if (!string.IsNullOrEmpty(request.UpdateQcQuestionDTO.Question))
            {
                qcQuestion.Question = request.UpdateQcQuestionDTO.Question;
            }

            if (request.UpdateQcQuestionDTO.IsRequired.HasValue)
            {
                qcQuestion.IsRequired = request.UpdateQcQuestionDTO.IsRequired.Value;
            }

            if (request.UpdateQcQuestionDTO.CorrectAnswer != null)
            {
                qcQuestion.CorrectAnswer = request.UpdateQcQuestionDTO.CorrectAnswer;
            }

            if (request.UpdateQcQuestionDTO.QcSectionId.HasValue)
            {
                var existingQcQuestion = await _qcSectionRepository.GetByIdAsync(request.UpdateQcQuestionDTO.QcSectionId.Value, cancellationToken);

                if (existingQcQuestion == null)
                {
                    return Result<object>.Failure("QcSection not found", HttpStatusCode.NotFound);
                }

                qcQuestion.QcSectionId = request.UpdateQcQuestionDTO.QcSectionId.Value;
            }

            if (request.UpdateQcQuestionDTO.QcAnswerTypeId.HasValue)
            {
                var existingQcAnswerType = await _qcAnswerTypeRepository.GetByIdAsync(request.UpdateQcQuestionDTO.QcAnswerTypeId.Value, cancellationToken);

                if (existingQcAnswerType == null)
                {
                    return Result<object>.Failure("QcAnswerType not found", HttpStatusCode.NotFound);
                }

                qcQuestion.QcAnswerTypeId = request.UpdateQcQuestionDTO.QcAnswerTypeId.Value;
            }

            if (request.UpdateQcQuestionDTO.Order.HasValue)
            {
                var existingQcQuestion = await _qcQuestionRepository.ExistsInSameOrderAsync(request.UpdateQcQuestionDTO.Order.Value, request.Id, qcQuestion.QcSectionId, cancellationToken);

                if (existingQcQuestion)
                {
                    return Result<object>.Failure("A question with the same order already exists", HttpStatusCode.Conflict);
                }

                qcQuestion.Order = request.UpdateQcQuestionDTO.Order.Value;
            }

            qcQuestion.UpdatedAt = DateTime.UtcNow;
            qcQuestion.UpdatedById = existingUser.Id;

            await _qcQuestionRepository.UpdateAsync(qcQuestion, cancellationToken);

            return Result<object>.Success(null, "QcQuestion updated successfully");
        }
    }
}
