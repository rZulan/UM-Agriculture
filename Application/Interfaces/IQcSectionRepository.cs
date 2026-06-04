using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface IQcSectionRepository
    {
        Task<List<QcSection>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<QcSection?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> ExistsInSameOrderAsync(int order, int? id, CancellationToken cancellationToken);
        Task AddAsync(QcSection qcSection, CancellationToken cancellationToken);
        Task UpdateAsync(QcSection qcSection, CancellationToken cancellationToken);
    }
}
