using Application.Interfaces;
using Application.Results;
using MediatR;
using System.Net;

namespace Application.Features.Products.Commands
{
    public record ToggleProductActiveCommand(int? UserId, int Id, bool IsActive) : IRequest<Result<object>>;
    public class ToggleProductActiveCommandHandler(IProductRepository productRepository, IUserRepository userRepository) : IRequestHandler<ToggleProductActiveCommand, Result<object>>
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(ToggleProductActiveCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingProduct = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingProduct == null)
            {
                return Result<object>.Failure("Product not found");
            }

            if (request.IsActive && existingProduct.IsActive)
            {
                return Result<object>.Failure("Product is already active");
            }

            if (!request.IsActive && !existingProduct.IsActive)
            {
                return Result<object>.Failure("Product is already archived");
            }

            existingProduct.IsActive = request.IsActive;
            existingProduct.UpdatedAt = DateTime.UtcNow;
            existingProduct.UpdatedById = existingUser.Id;

            await _productRepository.UpdateAsync(existingProduct, cancellationToken);

            var status = existingProduct.IsActive ? "restored" : "archived";

            return Result<object>.Success(existingProduct.Id, $"Product {status} successfully");
        }
    }
}
