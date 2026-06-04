using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<List<ProductCategory>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<ProductCategory?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(ProductCategory productCategory, CancellationToken cancellationToken);
        Task UpdateAsync(ProductCategory productCategory, CancellationToken cancellationToken);
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
