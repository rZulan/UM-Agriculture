using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<Role>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken);
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Role Role, CancellationToken cancellationToken);
        Task UpdateAsync(Role Role, CancellationToken cancellationToken);
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
