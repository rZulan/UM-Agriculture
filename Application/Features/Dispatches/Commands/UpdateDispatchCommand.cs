using Application.DTO.Dispatch;
using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Dispatches.Commands
{
    public record UpdateDispatchCommand(int? UserId, int Id, UpdateDispatchDTO UpdateDispatchDTO) : IRequest<Result<object>>;
    public class UpdateDispatchCommandHandler(IDispatchRepository dispatchRepository, IUserRepository userRepository) : IRequestHandler<UpdateDispatchCommand, Result<object>>
    {
        private readonly IDispatchRepository _dispatchRepository = dispatchRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(UpdateDispatchCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existing = await _dispatchRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existing == null)
            {
                return Result<object>.Failure("Dispatch not found", System.Net.HttpStatusCode.NotFound);
            }

            if (!string.IsNullOrEmpty(request.UpdateDispatchDTO.BatchNumber)) existing.BatchNumber = request.UpdateDispatchDTO.BatchNumber;
            if (request.UpdateDispatchDTO.ProductId.HasValue) existing.ProductId = request.UpdateDispatchDTO.ProductId.Value;
            if (request.UpdateDispatchDTO.FarmId.HasValue) existing.FarmId = request.UpdateDispatchDTO.FarmId.Value;
            if (request.UpdateDispatchDTO.QuantityOut.HasValue) existing.QuantityOut = request.UpdateDispatchDTO.QuantityOut.Value;
            if (request.UpdateDispatchDTO.QuantityReturn.HasValue) existing.QuantityReturn = request.UpdateDispatchDTO.QuantityReturn.Value;
            if (request.UpdateDispatchDTO.HarvestDate.HasValue) existing.HarvestDate = request.UpdateDispatchDTO.HarvestDate.Value;

            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedById = existingUser.Id;

            await _dispatchRepository.UpdateAsync(existing, cancellationToken);

            return Result<object>.Success(existing.Id, "Dispatch updated successfully");
        }
    }
}