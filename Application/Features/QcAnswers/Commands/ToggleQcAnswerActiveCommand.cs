using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcAnswers.Commands
{
    public record ToggleQcAnswerActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleQcAnswerActiveCommandHandler(IQcAnswerRepository qcAnswerRepository, IUserRepository userRepository) : IRequestHandler<ToggleQcAnswerActiveCommand, Result<object>>
    {
        private readonly IQcAnswerRepository _qcAnswerRepository = qcAnswerRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleQcAnswerActiveCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var qcAnswer = await _qcAnswerRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcAnswer == null)
            {
                return Result<object>.Failure("QcAnswer not found", HttpStatusCode.NotFound);
            }

            if (request.IsActive && qcAnswer.IsActive)
            {
                return Result<object>.Failure("QcAnswer is already active");
            }

            if (!request.IsActive && !qcAnswer.IsActive)
            {
                return Result<object>.Failure("QcAnswer is already archived");
            }

            qcAnswer.IsActive = request.IsActive;
            qcAnswer.UpdatedAt = DateTime.UtcNow;
            qcAnswer.UpdatedById = existingUser.Id;

            await _qcAnswerRepository.UpdateAsync(qcAnswer, cancellationToken);

            var status = qcAnswer.IsActive ? "restored" : "archived";

            return Result<object>.Success(qcAnswer.Id, $"QcAnswer {status} successfully");
        }
    }
}
