using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides data access operations for <see cref="Supplier"/> entities.
    /// </summary>
    public interface ISupplierRepository
    {
        /// <summary>Returns a filtered and sorted list of all suppliers.</summary>
        /// <param name="genericFiltersDTO">Search and pagination filters.</param>
        /// <param name="sort">Sort direction and field.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<List<Supplier>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);

        /// <summary>Returns the total count of suppliers matching the given filters.</summary>
        /// <param name="genericFiltersDTO">Filters to apply before counting.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);

        /// <summary>Returns a supplier by its ID, or <see langword="null"/> if not found.</summary>
        /// <param name="id">The supplier's unique identifier.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Supplier?> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>Returns a supplier matching the given name, or <see langword="null"/> if not found.</summary>
        /// <param name="name">The exact supplier name to search for.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Supplier?> GetByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>Persists a new supplier to the data store.</summary>
        /// <param name="supplier">The supplier entity to add.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task AddAsync(Supplier supplier, CancellationToken cancellationToken);

        /// <summary>Saves changes to an existing supplier.</summary>
        /// <param name="supplier">The supplier entity with updated values.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken);

        /// <summary>Checks whether another supplier (excluding the given ID) already uses the specified name.</summary>
        /// <param name="id">The ID of the supplier to exclude from the check.</param>
        /// <param name="name">The name to check for duplicates.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns><see langword="true"/> if a duplicate exists; otherwise <see langword="false"/>.</returns>
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
