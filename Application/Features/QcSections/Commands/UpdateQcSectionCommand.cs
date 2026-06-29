using Application.DTO.QcSection;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.QcSections.Commands
{
    /// <summary>Command to update an existing QC section.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the QC section to update.</param>
    /// <param name="UpdateQcSectionDTO">The updated QC section data.</param>
    public record UpdateQcSectionCommand(int? UserId, int Id, UpdateQcSectionDTO UpdateQcSectionDTO) : IRequest<Result<object>>;
    public class UpdateQcSectionCommandHandler(IQcSectionRepository qcSectionRepository, IUserRepository userRepository) : IRequestHandler<UpdateQcSectionCommand, Result<object>>
    {
        private readonly IQcSectionRepository _qcSectionRepository = qcSectionRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdateQcSectionCommand request, CancellationToken cancellationToken)
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

            if (!string.IsNullOrEmpty(request.UpdateQcSectionDTO.Name))
            {
                qcSection.Name = request.UpdateQcSectionDTO.Name;
            }

            if (!string.IsNullOrEmpty(request.UpdateQcSectionDTO.Description))
            {
                qcSection.Description = request.UpdateQcSectionDTO.Description;
            }

            if (request.UpdateQcSectionDTO.Order.HasValue)
            {
                var existingQcSection = await _qcSectionRepository.ExistsInSameOrderAsync(request.UpdateQcSectionDTO.Order.Value, request.Id, cancellationToken);

                if (existingQcSection)
                {
                    return Result<object>.Failure("A section with the same order already exists", HttpStatusCode.Conflict);
                }

                qcSection.Order = request.UpdateQcSectionDTO.Order.Value;
            }

            if (request.UpdateQcSectionDTO.QcFormId.HasValue)
            {
                qcSection.QcFormId = request.UpdateQcSectionDTO.QcFormId.Value;
            }

            qcSection.UpdatedAt = DateTime.UtcNow;
            qcSection.UpdatedById = existingUser.Id;

            await _qcSectionRepository.UpdateAsync(qcSection, cancellationToken);

            return Result<object>.Success(null, "QcSection updated successfully");
        }
    }
}
