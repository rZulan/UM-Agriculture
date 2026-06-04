using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface IPondRepository
    {
        Task<List<Pond>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<Pond?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Pond?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Pond pond, CancellationToken cancellationToken);
        Task UpdateAsync(Pond pond, CancellationToken cancellationToken);
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
