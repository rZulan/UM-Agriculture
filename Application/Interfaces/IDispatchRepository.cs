using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDispatchRepository
    {
        Task<List<Dispatch>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<List<Dispatch>> GetAllForQcAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<Dispatch?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> ExistsInSameBatchNumber(string batchNumber, CancellationToken cancellationToken);
        Task AddAsync(Dispatch dispatch, CancellationToken cancellationToken);
        Task UpdateAsync(Dispatch dispatch, CancellationToken cancellationToken);
    }
}