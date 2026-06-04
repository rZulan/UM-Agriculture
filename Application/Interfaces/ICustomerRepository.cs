using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Customer?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Customer customer, CancellationToken cancellationToken);
        Task UpdateAsync(Customer customer, CancellationToken cancellationToken);
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
