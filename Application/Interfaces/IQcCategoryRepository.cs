using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface IQcCategoryRepository
    {
        Task<List<QcCategory>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<QcCategory?> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}
