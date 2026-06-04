using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Category category, CancellationToken cancellationToken);
        Task UpdateAsync(Category category, CancellationToken cancellationToken);
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
