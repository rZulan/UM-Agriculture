using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface ISupplierRepository
    {
        Task<List<Supplier>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<Supplier?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Supplier?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Supplier supplier, CancellationToken cancellationToken);
        Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken);
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
