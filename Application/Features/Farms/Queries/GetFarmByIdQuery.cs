using Application.DTO.Farm;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Farms.Queries
{
    public record GetFarmByIdQuery(int Id) : IRequest<Result<GetFarmDTO>>;
    public class GetFarmByIdQueryHandler(IFarmRepository farmRepository) : IRequestHandler<GetFarmByIdQuery, Result<GetFarmDTO>>
    {
        private readonly IFarmRepository _farmRepository = farmRepository;

        public async Task<Result<GetFarmDTO>> Handle(GetFarmByIdQuery request, CancellationToken cancellationToken)
        {
            var farm = await _farmRepository.GetByIdAsync(request.Id, cancellationToken);

            if (farm == null)
            {
                return Result<GetFarmDTO>.Failure("Farm not found", HttpStatusCode.NotFound);
            }

            var result = new GetFarmDTO
            {
                Id = farm.Id,
                IsActive = farm.IsActive,
                Name = farm.Name,
                Address = farm.Address,
            };

            return Result<GetFarmDTO>.Success(result);
        }
    }
}