using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPendingAccountRepository
    {
        Task<List<PendingAccount>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<PendingAccount?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<PendingAccount?> GetByFullIdNoAsync(string employeePrefix, string employeeId, CancellationToken cancellationToken);
        Task AddAsync(PendingAccount PendingAccount, CancellationToken cancellationToken);
        Task UpdateAsync(PendingAccount PendingAccount, CancellationToken cancellationToken);
    }
}
