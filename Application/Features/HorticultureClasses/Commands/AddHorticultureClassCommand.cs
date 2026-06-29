using Application.DTO.HorticultureClass;
using Application.Interfaces;
using Application.Results;
using Domain.Entities.Masterlist;
using MediatR;
using System.Net;

namespace Application.Features.HorticultureClasses.Commands
{
    /// <summary>Command to create a new horticulture class.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="AddHorticultureClassDTO">The horticulture class data to be created.</param>
    public record AddHorticultureClassCommand(int? UserId, AddHorticultureClassDTO AddHorticultureClassDTO) : IRequest<Result<object>>;
    public class AddHorticultureClassCommandHandler(IHorticultureClassRepository horticultureClassRepository, IUserRepository userRepository) : IRequestHandler<AddHorticultureClassCommand, Result<object>>
    {
        private readonly IHorticultureClassRepository _horticultureClassRepository = horticultureClassRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(AddHorticultureClassCommand request, CancellationToken cancellationToken)
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

            var existingHorticultureClass = await _horticultureClassRepository.GetByNameAsync(request.AddHorticultureClassDTO.Name, cancellationToken);

            if (existingHorticultureClass != null)
            {
                return Result<object>.Failure("HorticultureClass already exists", HttpStatusCode.Conflict);
            }

            var horticultureClass = new HorticultureClass
            {
                Name = request.AddHorticultureClassDTO.Name,
                CreatedAt = DateTime.UtcNow,
                CreatedById = existingUser.Id
            };

            await _horticultureClassRepository.AddAsync(horticultureClass, cancellationToken);

            return Result<object>.Success(horticultureClass.Id, "HorticultureClass created successfully", HttpStatusCode.Created);
        }
    }
}
