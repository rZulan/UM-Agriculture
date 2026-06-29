using Application.DTO.Pond;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Ponds.Commands
{
    /// <summary>Command to update an existing pond.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="Id">The ID of the pond to update.</param>
    /// <param name="UpdatePondDTO">The updated pond data.</param>
    public record UpdatePondCommand(int? UserId, int Id, UpdatePondDTO UpdatePondDTO) : IRequest<Result<object>>;
    public class UpdatePondCommandHandler(IPondRepository pondRepository, IUserRepository userRepository) : IRequestHandler<UpdatePondCommand, Result<object>>
    {
        private readonly IPondRepository _pondRepository = pondRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdatePondCommand request, CancellationToken cancellationToken)
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

            var existingPond = await _pondRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingPond == null)
            {
                return Result<object>.Failure("Pond not found", System.Net.HttpStatusCode.NotFound);
            }

            if (!string.IsNullOrEmpty(request.UpdatePondDTO.Name))
            {
                var existingName = await _pondRepository.CheckDuplicateAsync(request.Id, request.UpdatePondDTO.Name, cancellationToken);

                if (existingName)
                {
                    return Result<object>.Failure("Pond name already exists", System.Net.HttpStatusCode.BadRequest);
                }

                existingPond.Name = request.UpdatePondDTO.Name;
            }

            existingPond.UpdatedAt = DateTime.UtcNow;
            existingPond.UpdatedById = existingUser.Id;

            await _pondRepository.UpdateAsync(existingPond, cancellationToken);

            return Result<object>.Success(null, "Pond updated successfully");
        }
    }
}
