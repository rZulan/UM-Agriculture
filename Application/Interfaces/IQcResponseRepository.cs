using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IQcResponseRepository
    {
        Task<List<QcResponse>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, string qcType, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<QcResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> ExistsInSameDispatchId(int dispatchId, int? id, CancellationToken cancellationToken);
        Task AddAsync(QcResponse qcResponse, CancellationToken cancellationToken);
        Task UpdateAsync(QcResponse qcResponse, CancellationToken cancellationToken);
    }
}
