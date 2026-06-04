using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface IUomRepository
    {
        Task<List<Uom>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<Uom?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Uom?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Uom Uom, CancellationToken cancellationToken);
        Task UpdateAsync(Uom Uom, CancellationToken cancellationToken);
        Task<bool> CheckDuplicateAsync(int id, string itemCode, CancellationToken cancellationToken);
    }
}
