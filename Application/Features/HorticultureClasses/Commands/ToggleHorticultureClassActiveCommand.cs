using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.HorticultureClasses.Commands
{
    public record ToggleHorticultureClassActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleHorticultureClassActiveCommandHandler(IHorticultureClassRepository horticultureClassRepository, IUserRepository userRepository) : IRequestHandler<ToggleHorticultureClassActiveCommand, Result<object>>
    {
        private readonly IHorticultureClassRepository _horticultureClassRepository = horticultureClassRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleHorticultureClassActiveCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

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
