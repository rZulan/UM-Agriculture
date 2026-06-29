using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides data access operations for <see cref="Department"/> entities.
    /// </summary>
    public interface IDepartmentRepository
    {
        /// <summary>Returns a filtered and sorted list of all departments.</summary>
        /// <param name="genericFiltersDTO">Search and pagination filters.</param>
        /// <param name="sort">Sort direction and field.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<List<Department>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        /// <summary>Returns the total count of departments matching the given filters.</summary>
        /// <param name="genericFiltersDTO">Filters to apply before counting.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        /// <summary>Returns a department by its ID, or <see langword="null"/> if not found.</summary>
        /// <param name="id">The department's unique identifier.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken);
        /// <summary>Returns a department matching the given name, or <see langword="null"/> if not found.</summary>
        /// <param name="name">The exact department name to search for.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken);
        /// <summary>Persists a new department to the data store.</summary>
        /// <param name="department">The department entity to add.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task AddAsync(Department department, CancellationToken cancellationToken);
        /// <summary>Saves changes to an existing department.</summary>
        /// <param name="department">The department entity with updated values.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task UpdateAsync(Department department, CancellationToken cancellationToken);
        /// <summary>Checks whether another department (excluding the given ID) already uses the specified name.</summary>
        /// <param name="id">The ID of the department to exclude from the check.</param>
        /// <param name="name">The name to check for duplicates.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns><see langword="true"/> if a duplicate exists; otherwise <see langword="false"/>.</returns>
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
