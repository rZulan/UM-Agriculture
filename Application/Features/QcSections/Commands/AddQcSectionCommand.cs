using Application.DTO.QcSection;
using Application.Interfaces;
using Application.Results;
using Domain.Entities.Masterlist;
using MediatR;
using System.Net;

namespace Application.Features.QcSections.Commands
{
    public record AddQcSectionCommand(int? UserId, AddQcSectionDTO AddQcSectionDTO) : IRequest<Result<object>>;
    public class AddQcSectionCommandHandler(IQcSectionRepository qcSectionRepository, IUserRepository userRepository) : IRequestHandler<AddQcSectionCommand, Result<object>>
    {
        private readonly IQcSectionRepository _qcSectionRepository = qcSectionRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(AddQcSectionCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

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
