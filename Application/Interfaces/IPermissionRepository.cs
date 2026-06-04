using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<Permission?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Permission Permission, CancellationToken cancellationToken);
        Task UpdateAsync(Permission Permission, CancellationToken cancellationToken);
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
