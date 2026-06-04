using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Department department, CancellationToken cancellationToken);
        Task UpdateAsync(Department department, CancellationToken cancellationToken);
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
