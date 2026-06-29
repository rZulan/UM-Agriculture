using Application.DTO.QcSection;
using Application.Interfaces;
using Application.Results;
using Domain.Entities.Masterlist;
using MediatR;
using System.Net;

namespace Application.Features.QcSections.Commands
{
    /// <summary>Command to add a new section to a QC form.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="AddQcSectionDTO">The QC section data to be created.</param>
    public record AddQcSectionCommand(int? UserId, AddQcSectionDTO AddQcSectionDTO) : IRequest<Result<object>>;
    public class AddQcSectionCommandHandler(IQcSectionRepository qcSectionRepository, IUserRepository userRepository) : IRequestHandler<AddQcSectionCommand, Result<object>>
    {
        private readonly IQcSectionRepository _qcSectionRepository = qcSectionRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(AddQcSectionCommand request, CancellationToken cancellationToken)
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

            var existingQcSection = await _qcSectionRepository.ExistsInSameOrderAsync(request.AddQcSectionDTO.Order, null, cancellationToken);

            if (existingQcSection)
            {
                return Result<object>.Failure("A QcSection with the specified order already exists.", HttpStatusCode.Conflict);
            }

            var qcSection = new QcSection
            {
                Name = request.AddQcSectionDTO.Name,
                Description = request.AddQcSectionDTO.Description,
                Order = request.AddQcSectionDTO.Order,
                QcFormId = request.AddQcSectionDTO.QcFormId,
                CreatedAt = DateTime.UtcNow,
                CreatedById = existingUser.Id
            };

            await _qcSectionRepository.AddAsync(qcSection, cancellationToken);

            return Result<object>.Success(qcSection.Id, "QcSection created successfully", HttpStatusCode.Created);
        }
    }
}
