using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcQuestions.Commands
{
    public record ToggleQcQuestionActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleQcQuestionActiveCommandHandler(IQcQuestionRepository qcQuestionRepository, IUserRepository userRepository) : IRequestHandler<ToggleQcQuestionActiveCommand, Result<object>>
    {
        private readonly IQcQuestionRepository _qcQuestionRepository = qcQuestionRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleQcQuestionActiveCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var qcQuestion = await _qcQuestionRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcQuestion == null)
            {
                return Result<object>.Failure("QcQuestion not found", HttpStatusCode.NotFound);
            }

            if (request.IsActive && qcQuestion.IsActive)
            {
                return Result<object>.Failure("QcQuestion is already active");
            }

            if (!request.IsActive && !qcQuestion.IsActive)
            {
                return Result<object>.Failure("QcQuestion is already archived");
            }

            qcQuestion.IsActive = request.IsActive;
            qcQuestion.UpdatedAt = DateTime.UtcNow;
            qcQuestion.UpdatedById = existingUser.Id;

            await _qcQuestionRepository.UpdateAsync(qcQuestion, cancellationToken);

            var status = qcQuestion.IsActive ? "restored" : "archived";

            return Result<object>.Success(qcQuestion.Id, $"QcQuestion {status} successfully");
        }
    }
}
