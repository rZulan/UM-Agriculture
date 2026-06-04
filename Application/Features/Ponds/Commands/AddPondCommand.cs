using Application.DTO.Pond;
using Application.Interfaces;
using Application.Results;
using Domain.Entities.Masterlist;
using MediatR;
using System.Net;

namespace Application.Features.Ponds.Commands
{
    public record AddPondCommand(int? UserId, AddPondDTO AddPondDTO) : IRequest<Result<object>>;
    public class AddPondCommandHandler(IPondRepository pondRepository, IUserRepository userRepository) : IRequestHandler<AddPondCommand, Result<object>>
    {
        private readonly IPondRepository _pondRepository = pondRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(AddPondCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingPond = await _pondRepository.GetByNameAsync(request.AddPondDTO.Name, cancellationToken);

            if (existingPond != null)
            {
                return Result<object>.Failure("Pond already exists", HttpStatusCode.Conflict);
            }

            var pond = new Pond
            {
                Name = request.AddPondDTO.Name,
                CreatedAt = DateTime.UtcNow,
                CreatedById = existingUser.Id
            };

            await _pondRepository.AddAsync(pond, cancellationToken);

            return Result<object>.Success(pond.Id, "Pond created successfully", HttpStatusCode.Created);
        }
    }
}
