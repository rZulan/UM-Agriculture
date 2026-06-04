using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcSections.Commands
{
    public record ToggleQcSectionActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleQcSectionActiveCommandHandler(IQcSectionRepository qcSectionRepository, IUserRepository userRepository) : IRequestHandler<ToggleQcSectionActiveCommand, Result<object>>
    {
        private readonly IQcSectionRepository _qcSectionRepository = qcSectionRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleQcSectionActiveCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

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
