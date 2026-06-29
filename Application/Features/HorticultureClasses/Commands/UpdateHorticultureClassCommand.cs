using Application.DTO.HorticultureClass;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.HorticultureClasses.Commands
{
    /// <summary>Command to update an existing horticulture class.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the horticulture class to update.</param>
    /// <param name="UpdateHorticultureClassDTO">The updated horticulture class data.</param>
    public record UpdateHorticultureClassCommand(int? UserId, int Id, UpdateHorticultureClassDTO UpdateHorticultureClassDTO) : IRequest<Result<object>>;
    public class UpdateHorticultureClassCommandHandler(IHorticultureClassRepository horticultureClassRepository, IUserRepository userRepository) : IRequestHandler<UpdateHorticultureClassCommand, Result<object>>
    {
        private readonly IHorticultureClassRepository _horticultureClassRepository = horticultureClassRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdateHorticultureClassCommand request, CancellationToken cancellationToken)
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

            var existingHorticultureClass = await _horticultureClassRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingHorticultureClass == null)
            {
                return Result<object>.Failure("HorticultureClass not found", System.Net.HttpStatusCode.NotFound);
            }

            if (!string.IsNullOrEmpty(request.UpdateHorticultureClassDTO.Name))
            {
                var existingName = await _horticultureClassRepository.CheckDuplicateAsync(request.Id, request.UpdateHorticultureClassDTO.Name, cancellationToken);

                if (existingName)
                {
                    return Result<object>.Failure("HorticultureClass name already exists", System.Net.HttpStatusCode.BadRequest);
                }

                existingHorticultureClass.Name = request.UpdateHorticultureClassDTO.Name;
            }

            existingHorticultureClass.UpdatedAt = DateTime.UtcNow;
            existingHorticultureClass.UpdatedById = existingUser.Id;

            await _horticultureClassRepository.UpdateAsync(existingHorticultureClass, cancellationToken);

            return Result<object>.Success(null, "HorticultureClass updated successfully");
        }
    }
}
