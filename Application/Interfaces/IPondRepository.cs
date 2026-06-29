using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides data access operations for <see cref="Pond"/> entities.
    /// </summary>
    public interface IPondRepository
    {
        /// <summary>Returns a filtered and sorted list of all ponds.</summary>
        /// <param name="genericFiltersDTO">Search and pagination filters.</param>
        /// <param name="sort">Sort direction and field.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<List<Pond>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        /// <summary>Returns the total count of ponds matching the given filters.</summary>
        /// <param name="genericFiltersDTO">Filters to apply before counting.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        /// <summary>Returns a pond by its ID, or <see langword="null"/> if not found.</summary>
        /// <param name="id">The pond's unique identifier.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Pond?> GetByIdAsync(int id, CancellationToken cancellationToken);
        /// <summary>Returns a pond matching the given name, or <see langword="null"/> if not found.</summary>
        /// <param name="name">The exact pond name to search for.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Pond?> GetByNameAsync(string name, CancellationToken cancellationToken);
        /// <summary>Persists a new pond to the data store.</summary>
        /// <param name="pond">The pond entity to add.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task AddAsync(Pond pond, CancellationToken cancellationToken);
        /// <summary>Saves changes to an existing pond.</summary>
        /// <param name="pond">The pond entity with updated values.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task UpdateAsync(Pond pond, CancellationToken cancellationToken);
        /// <summary>Checks whether another pond (excluding the given ID) already uses the specified name.</summary>
        /// <param name="id">The ID of the pond to exclude from the check.</param>
        /// <param name="name">The name to check for duplicates.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns><see langword="true"/> if a duplicate exists; otherwise <see langword="false"/>.</returns>
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
