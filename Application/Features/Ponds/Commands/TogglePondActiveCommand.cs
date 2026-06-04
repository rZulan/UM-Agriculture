using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Ponds.Commands
{
    public record TogglePondActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class TogglePondActiveCommandHandler(IPondRepository pondRepository, IUserRepository userRepository) : IRequestHandler<TogglePondActiveCommand, Result<object>>
    {
        private readonly IPondRepository _pondRepository = pondRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(TogglePondActiveCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingPond = await _pondRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingPond == null)
            {
                return Result<object>.Failure("Pond not found");
            }

            if (request.IsActive && existingPond.IsActive)
            {
                return Result<object>.Failure("Pond is already active");
            }

            if (!request.IsActive && !existingPond.IsActive)
            {
                return Result<object>.Failure("Pond is already archived");
            }

            existingPond.IsActive = request.IsActive;
            existingPond.UpdatedAt = DateTime.UtcNow;
            existingPond.UpdatedById = existingUser.Id;

            await _pondRepository.UpdateAsync(existingPond, cancellationToken);

            var status = existingPond.IsActive ? "restored" : "archived";

            return Result<object>.Success(existingPond.Id, $"Pond {status} successfully");
        }
    }
}
