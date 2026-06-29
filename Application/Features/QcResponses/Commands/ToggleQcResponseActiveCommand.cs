using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcResponses.Commands
{
    /// <summary>Command to activate or deactivate a QC response.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the QC response to toggle.</param>
    /// <param name="IsActive">The desired active state.</param>
    public record ToggleQcResponseActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleQcResponseActiveCommandHandler(IQcResponseRepository qcResponseRepository, IUserRepository userRepository) : IRequestHandler<ToggleQcResponseActiveCommand, Result<object>>
    {
        private readonly IQcResponseRepository _qcResponseRepository = qcResponseRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleQcResponseActiveCommand request, CancellationToken cancellationToken)
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

            if (request.IsActive && qcResponse.IsActive)
            {
                return Result<object>.Failure("QcResponse is already active");
            }

            if (!request.IsActive && !qcResponse.IsActive)
            {
                return Result<object>.Failure("QcResponse is already archived");
            }

            qcResponse.IsActive = request.IsActive;
            qcResponse.UpdatedAt = DateTime.UtcNow;
            qcResponse.UpdatedById = existingUser.Id;

            await _qcResponseRepository.UpdateAsync(qcResponse, cancellationToken);

            var status = qcResponse.IsActive ? "restored" : "archived";

            return Result<object>.Success(qcResponse.Id, $"QcResponse {status} successfully");
        }
    }
}
