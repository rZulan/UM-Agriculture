using Application.DTO.Pond;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Ponds.Queries
{
    public record GetPondByIdQuery(int Id) : IRequest<Result<GetPondDTO>>;
    public class GetPondByIdQueryHandler(IPondRepository pondRepository) : IRequestHandler<GetPondByIdQuery, Result<GetPondDTO>>
    {
        private readonly IPondRepository _pondRepository = pondRepository;

        public async Task<Result<GetPondDTO>> Handle(GetPondByIdQuery request, CancellationToken cancellationToken)
        {
            var pond = await _pondRepository.GetByIdAsync(request.Id, cancellationToken);

            if (pond == null)
            {
                return Result<GetPondDTO>.Failure("Pond not found", HttpStatusCode.NotFound);
            }

            var result = new GetPondDTO
            {
                Id = pond.Id,
                IsActive = pond.IsActive,
                Name = pond.Name
            };

            return Result<GetPondDTO>.Success(result);
        }
    }
}
