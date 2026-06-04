using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort userSort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<User?> GetByFullIdNoAsync(string employeePrefix, string employeeId, CancellationToken cancellationToken);
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
        Task AddAsync(User user, CancellationToken cancellationToken);
        Task UpdateAsync(User user, CancellationToken cancellationToken);
        Task<bool> CheckDuplicateAsync(int id, string username, CancellationToken cancellationToken);
    }
}
