using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface IHorticultureClassRepository
    {
        Task<List<HorticultureClass>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<HorticultureClass?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<HorticultureClass?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(HorticultureClass HorticultureClass, CancellationToken cancellationToken);
        Task UpdateAsync(HorticultureClass HorticultureClass, CancellationToken cancellationToken);
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
