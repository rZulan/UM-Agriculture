using Application.DTO.QcResponse;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcResponses.Commands
{
    /// <summary>Command to update an existing QC response.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the QC response to update.</param>
    /// <param name="UpdateQcResponseDTO">The updated QC response data.</param>
    public record UpdateQcResponseCommand(int? UserId, int Id, UpdateQcResponseDTO UpdateQcResponseDTO) : IRequest<Result<object>>;
    public class UpdateQcResponseCommandHandler(IQcResponseRepository qcResponseRepository, IUserRepository userRepository) : IRequestHandler<UpdateQcResponseCommand, Result<object>>
    {
        private readonly IQcResponseRepository _qcResponseRepository = qcResponseRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdateQcResponseCommand request, CancellationToken cancellationToken)
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

            var qcResponse = await _qcResponseRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcResponse == null)
            {
                return Result<object>.Failure("QcResponse not found", HttpStatusCode.NotFound);
            }

            if (request.UpdateQcResponseDTO.QcFormId.HasValue)
            {
                qcResponse.QcFormId = request.UpdateQcResponseDTO.QcFormId.Value;
            }

            if (request.UpdateQcResponseDTO.ResponderId.HasValue)
            {
                qcResponse.ResponderId = request.UpdateQcResponseDTO.ResponderId.Value;
            }

            qcResponse.UpdatedAt = DateTime.UtcNow;
            qcResponse.UpdatedById = existingUser.Id;

            await _qcResponseRepository.UpdateAsync(qcResponse, cancellationToken);

            return Result<object>.Success(null, "QcResponse updated successfully");
        }
    }
}
