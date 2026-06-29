using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.HorticultureClasses.Commands
{
    /// <summary>Command to activate or deactivate a horticulture class.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the horticulture class to toggle.</param>
    /// <param name="IsActive">The desired active state.</param>
    public record ToggleHorticultureClassActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleHorticultureClassActiveCommandHandler(IHorticultureClassRepository horticultureClassRepository, IUserRepository userRepository) : IRequestHandler<ToggleHorticultureClassActiveCommand, Result<object>>
    {
        private readonly IHorticultureClassRepository _horticultureClassRepository = horticultureClassRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleHorticultureClassActiveCommand request, CancellationToken cancellationToken)
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
                return Result<object>.Failure("Horticulture Class not found");
            }

            if (request.IsActive && existingHorticultureClass.IsActive)
            {
                return Result<object>.Failure("Horticulture Class is already active");
            }

            if (!request.IsActive && !existingHorticultureClass.IsActive)
            {
                return Result<object>.Failure("Horticulture Class is already archived");
            }

            existingHorticultureClass.IsActive = request.IsActive;
            existingHorticultureClass.UpdatedAt = DateTime.UtcNow;
            existingHorticultureClass.UpdatedById = existingUser.Id;

            await _horticultureClassRepository.UpdateAsync(existingHorticultureClass, cancellationToken);

            var status = existingHorticultureClass.IsActive ? "restored" : "archived";

            return Result<object>.Success(existingHorticultureClass.Id, $"Horticulture Class {status} successfully");
        }
    }
}
