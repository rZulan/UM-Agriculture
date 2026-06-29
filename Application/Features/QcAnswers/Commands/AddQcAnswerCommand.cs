using Application.DTO.QcAnswer;
using Application.Interfaces;
using Application.Results;
using Domain.Entities;
using MediatR;
using System.Net;

namespace Application.Features.QcAnswers.Commands
{
    /// <summary>Command to submit a new QC answer.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="AddQcAnswerDTO">The QC answer data to be created.</param>
    public record AddQcAnswerCommand(int? UserId, AddQcAnswerDTO AddQcAnswerDTO) : IRequest<Result<object>>;
    public class AddQcAnswerCommandHandler(IQcAnswerRepository qcAnswerRepository, IUserRepository userRepository) : IRequestHandler<AddQcAnswerCommand, Result<object>>
    {
        private readonly IQcAnswerRepository _qcAnswerRepository = qcAnswerRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(AddQcAnswerCommand request, CancellationToken cancellationToken)
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

            var qcAnswer = new QcAnswer
            {
                Answer = request.AddQcAnswerDTO.Answer,
                QcResponseId = request.AddQcAnswerDTO.QcResponseId,
                QcQuestionId = request.AddQcAnswerDTO.QcQuestionId,
                CreatedAt = DateTime.UtcNow,
                CreatedById = existingUser.Id
            };

            await _qcAnswerRepository.AddAsync(qcAnswer, cancellationToken);

            return Result<object>.Success(qcAnswer.Id, "QcAnswer created successfully", HttpStatusCode.Created);
        }
    }
}
