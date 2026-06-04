using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Farms.Commands
{
    public record ToggleFarmActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleFarmActiveCommandHandler(IFarmRepository farmRepository, IUserRepository userRepository) : IRequestHandler<ToggleFarmActiveCommand, Result<object>>
    {
        private readonly IFarmRepository _farmRepository = farmRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleFarmActiveCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingFarm = await _farmRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingFarm == null)
            {
                return Result<object>.Failure("Farm not found");
            }

            if (request.IsActive && existingFarm.IsActive)
            {
                return Result<object>.Failure("Farm is already active");
            }

            if (!request.IsActive && !existingFarm.IsActive)
            {
                return Result<object>.Failure("Farm is already archived");
            }

            existingFarm.IsActive = request.IsActive;
            existingFarm.UpdatedAt = DateTime.UtcNow;
            existingFarm.UpdatedById = existingUser.Id;

            await _farmRepository.UpdateAsync(existingFarm, cancellationToken);

            var status = existingFarm.IsActive ? "restored" : "archived";

            return Result<object>.Success(existingFarm.Id, $"Farm {status} successfully");
        }
    }
}
