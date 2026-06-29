using Application.DTO.HorticultureClass;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.HorticultureClasses.Queries
{
    /// <summary>Query to retrieve a single horticulture class by its ID.</summary>
    /// <param name="Id">The unique identifier of the horticulture class to retrieve.</param>
    public record GetHorticultureClassByIdQuery(int Id) : IRequest<Result<GetHorticultureClassDTO>>;
    public class GetHorticultureClassByIdQueryHandler(IHorticultureClassRepository horticultureClassRepository) : IRequestHandler<GetHorticultureClassByIdQuery, Result<GetHorticultureClassDTO>>
    {
        private readonly IHorticultureClassRepository _horticultureClassRepository = horticultureClassRepository;

        public async Task<Result<GetHorticultureClassDTO>> Handle(GetHorticultureClassByIdQuery request, CancellationToken cancellationToken)
        {
            var horticultureClass = await _horticultureClassRepository.GetByIdAsync(request.Id, cancellationToken);

            if (horticultureClass == null)
            {
                return Result<GetHorticultureClassDTO>.Failure("HorticultureClass not found", HttpStatusCode.NotFound);
            }

            var result = new GetHorticultureClassDTO
            {
                Id = horticultureClass.Id,
                IsActive = horticultureClass.IsActive,
                Name = horticultureClass.Name,
            };

            return Result<GetHorticultureClassDTO>.Success(result);
        }
    }
}