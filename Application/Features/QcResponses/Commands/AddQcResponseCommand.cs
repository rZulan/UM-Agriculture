using Application.DTO.QcResponse;
using Application.Interfaces;
using Application.Results;
using Domain.Entities;
using MediatR;
using System.Net;

namespace Application.Features.QcResponses.Commands
{
    public record AddQcResponseCommand(int? UserId, AddQcResponseDTO AddQcResponseDTO) : IRequest<Result<object>>;
    public class AddQcResponseCommandHandler(IQcResponseRepository qcResponseRepository, IQcAnswerRepository qcAnswerRepository, IUserRepository userRepository, IDispatchRepository dispatchRepository) : IRequestHandler<AddQcResponseCommand, Result<object>>
    {
        private readonly IQcResponseRepository _qcResponseRepository = qcResponseRepository;
        private readonly IQcAnswerRepository _qcAnswerRepository = qcAnswerRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IDispatchRepository _dispatchRepository = dispatchRepository;

        public async Task<Result<object>> Handle(AddQcResponseCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            List<int> questionIdWithNoAnswers = [.. request.AddQcResponseDTO.QcAnswers.Where(x => string.IsNullOrEmpty(x.Answer)).Select(a => a.QcQuestionId)];

            var noAnswerForRequiredQuestion = await _qcAnswerRepository.NoAnswerForRequiredQuestion(questionIdWithNoAnswers, cancellationToken);

            if (noAnswerForRequiredQuestion)
            {
                return Result<object>.Failure("Not all required questions have been answered", HttpStatusCode.BadRequest);
            }

            if (request.AddQcResponseDTO.DispatchId.HasValue)
            {
                var existingQcDispatchResponse = await _qcResponseRepository.ExistsInSameDispatchId((int)request.AddQcResponseDTO.DispatchId, null, cancellationToken);
                var dispatch = await _dispatchRepository.GetByIdAsync((int)request.AddQcResponseDTO.DispatchId, cancellationToken);

                if (existingQcDispatchResponse)
                {
                    return Result<object>.Failure("There is already a response for this Dispatch Form.", HttpStatusCode.Conflict);
                }

                if (dispatch == null)
                {
                    return Result<object>.Failure("Dispatch Form not found", HttpStatusCode.NotFound);
                }

                var qcResponse = new QcResponse
                {
                    QcFormId = request.AddQcResponseDTO.QcFormId,
                    ResponderId = existingUser.Id,
                    DispatchId = request.AddQcResponseDTO.DispatchId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = existingUser.Id
                };

                await _qcResponseRepository.AddAsync(qcResponse, cancellationToken);

                qcResponse.QcAnswers = [.. request.AddQcResponseDTO.QcAnswers.Select(a => new QcAnswer
                {
                    QcResponseId = qcResponse.Id,
                    QcQuestionId = a.QcQuestionId,
                    Answer = a.Answer,
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = existingUser.Id
                })];

                await _qcResponseRepository.UpdateAsync(qcResponse, cancellationToken);

                return Result<object>.Success(qcResponse.Id, "QcResponse created successfully", HttpStatusCode.Created);
            }
            return Result<object>.Failure("Invalid QcResponse data", HttpStatusCode.BadRequest);
        }
    }
}
