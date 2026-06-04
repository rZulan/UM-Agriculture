using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface IFarmRepository
    {
        Task<List<Farm>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<Farm?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Farm?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Farm Farm, CancellationToken cancellationToken);
        Task UpdateAsync(Farm Farm, CancellationToken cancellationToken);
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
