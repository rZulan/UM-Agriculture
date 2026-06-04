using Application.DTO.Farm;
using Application.Interfaces;
using Application.Results;
using Domain.Entities.Masterlist;
using MediatR;
using System.Net;

namespace Application.Features.Farms.Commands
{
    public record AddFarmCommand(int? UserId, AddFarmDTO AddFarmDTO) : IRequest<Result<object>>;
    public class AddFarmCommandHandler(IFarmRepository farmRepository, IUserRepository userRepository) : IRequestHandler<AddFarmCommand, Result<object>>
    {
        private readonly IFarmRepository _farmRepository = farmRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(AddFarmCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingFarm = await _farmRepository.GetByNameAsync(request.AddFarmDTO.Name, cancellationToken);

            if (existingFarm != null)
            {
                return Result<object>.Failure("Farm already exists", HttpStatusCode.Conflict);
            }

            var farm = new Farm
            {
                Name = request.AddFarmDTO.Name,
                Address = request.AddFarmDTO.Address,
                CreatedAt = DateTime.UtcNow,
                CreatedById = existingUser.Id
            };

            await _farmRepository.AddAsync(farm, cancellationToken);

            return Result<object>.Success(farm.Id, "Farm created successfully", HttpStatusCode.Created);
        }
    }
}
