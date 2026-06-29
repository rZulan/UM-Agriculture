using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcSections.Commands
{
    /// <summary>Command to activate or deactivate a QC section.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the QC section to toggle.</param>
    /// <param name="IsActive">The desired active state.</param>
    public record ToggleQcSectionActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleQcSectionActiveCommandHandler(IQcSectionRepository qcSectionRepository, IUserRepository userRepository) : IRequestHandler<ToggleQcSectionActiveCommand, Result<object>>
    {
        private readonly IQcSectionRepository _qcSectionRepository = qcSectionRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleQcSectionActiveCommand request, CancellationToken cancellationToken)
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

            var qcSection = await _qcSectionRepository.GetByIdAsync(request.Id, cancellationToken);

            if (qcSection == null)
            {
                return Result<object>.Failure("QcSection not found", HttpStatusCode.NotFound);
            }

            if (request.IsActive && qcSection.IsActive)
            {
                return Result<object>.Failure("QcSection is already active");
            }

            if (!request.IsActive && !qcSection.IsActive)
            {
                return Result<object>.Failure("QcSection is already archived");
            }

            qcSection.IsActive = request.IsActive;
            qcSection.UpdatedAt = DateTime.UtcNow;
            qcSection.UpdatedById = existingUser.Id;

            await _qcSectionRepository.UpdateAsync(qcSection, cancellationToken);

            var status = qcSection.IsActive ? "restored" : "archived";

            return Result<object>.Success(qcSection.Id, $"QcSection {status} successfully");
        }
    }
}
