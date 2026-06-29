using Application.DTO.QcAnswer;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcAnswers.Commands
{
    /// <summary>Command to update an existing QC answer.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the QC answer to update.</param>
    /// <param name="UpdateQcAnswerDTO">The updated QC answer data.</param>
    public record UpdateQcAnswerCommand(int? UserId, int Id, UpdateQcAnswerDTO UpdateQcAnswerDTO) : IRequest<Result<object>>;
    public class UpdateQcAnswerCommandHandler(IQcAnswerRepository qcAnswerRepository, IUserRepository userRepository) : IRequestHandler<UpdateQcAnswerCommand, Result<object>>
    {
        private readonly IQcAnswerRepository _qcAnswerRepository = qcAnswerRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdateQcAnswerCommand request, CancellationToken cancellationToken)
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

            var qcAnswer = await _qcAnswerRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcAnswer == null)
            {
                return Result<object>.Failure("QcAnswer not found", HttpStatusCode.NotFound);
            }

            if (request.UpdateQcAnswerDTO.Answer != null)
            {
                qcAnswer.Answer = request.UpdateQcAnswerDTO.Answer;
            }

            if (request.UpdateQcAnswerDTO.QcResponseId.HasValue)
            {
                qcAnswer.QcResponseId = request.UpdateQcAnswerDTO.QcResponseId.Value;
            }

            if (request.UpdateQcAnswerDTO.QcQuestionId.HasValue)
            {
                qcAnswer.QcQuestionId = request.UpdateQcAnswerDTO.QcQuestionId.Value;
            }

            qcAnswer.UpdatedAt = DateTime.UtcNow;
            qcAnswer.UpdatedById = existingUser.Id;

            await _qcAnswerRepository.UpdateAsync(qcAnswer, cancellationToken);

            return Result<object>.Success(null, "QcAnswer updated successfully");
        }
    }
}
