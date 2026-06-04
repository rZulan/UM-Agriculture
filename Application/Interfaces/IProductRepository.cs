using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Product?> GetByItemCodeAsync(string itemCode, CancellationToken cancellationToken);
        Task AddAsync(Product Product, CancellationToken cancellationToken);
        Task UpdateAsync(Product Product, CancellationToken cancellationToken);
        Task<bool> CheckDuplicateAsync(int id, string itemCode, CancellationToken cancellationToken);
    }
}
