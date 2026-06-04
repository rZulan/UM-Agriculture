using Application.DTO.Farm;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Farms.Commands
{
    public record UpdateFarmCommand(int? UserId, int Id, UpdateFarmDTO UpdateFarmDTO) : IRequest<Result<object>>;
    public class UpdateFarmCommandHandler(IFarmRepository farmRepository, IUserRepository userRepository) : IRequestHandler<UpdateFarmCommand, Result<object>>
    {
        private readonly IFarmRepository _farmRepository = farmRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdateFarmCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingFarm = await _farmRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingFarm == null)
            {
                return Result<object>.Failure("Farm not found", System.Net.HttpStatusCode.NotFound);
            }

            if (!string.IsNullOrEmpty(request.UpdateFarmDTO.Name))
            {
                var existingName = await _farmRepository.CheckDuplicateAsync(request.Id, request.UpdateFarmDTO.Name, cancellationToken);

                if (existingName)
                {
                    return Result<object>.Failure("Farm name already exists", System.Net.HttpStatusCode.BadRequest);
                }

                existingFarm.Name = request.UpdateFarmDTO.Name;
            }

            if (!string.IsNullOrEmpty(request.UpdateFarmDTO.Address))
            {
                existingFarm.Address = request.UpdateFarmDTO.Address;
            }

            existingFarm.UpdatedAt = DateTime.UtcNow;
            existingFarm.UpdatedById = existingUser.Id;

            await _farmRepository.UpdateAsync(existingFarm, cancellationToken);

            return Result<object>.Success(null, "Farm updated successfully");
        }
    }
}
