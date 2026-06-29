using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides data access operations for <see cref="Farm"/> entities.
    /// </summary>
    public interface IFarmRepository
    {
        /// <summary>Returns a filtered and sorted list of all farms.</summary>
        /// <param name="genericFiltersDTO">Search and pagination filters.</param>
        /// <param name="sort">Sort direction and field.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<List<Farm>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);

        /// <summary>Returns the total count of farms matching the given filters.</summary>
        /// <param name="genericFiltersDTO">Filters to apply before counting.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);

        /// <summary>Returns a farm by its ID, or <see langword="null"/> if not found.</summary>
        /// <param name="id">The farm's unique identifier.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Farm?> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>Returns a farm matching the given name, or <see langword="null"/> if not found.</summary>
        /// <param name="name">The exact farm name to search for.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Farm?> GetByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>Persists a new farm to the data store.</summary>
        /// <param name="Farm">The farm entity to add.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task AddAsync(Farm Farm, CancellationToken cancellationToken);

        /// <summary>Saves changes to an existing farm.</summary>
        /// <param name="Farm">The farm entity with updated values.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task UpdateAsync(Farm Farm, CancellationToken cancellationToken);

        /// <summary>Checks whether another farm (excluding the given ID) already uses the specified name.</summary>
        /// <param name="id">The ID of the farm to exclude from the check.</param>
        /// <param name="name">The name to check for duplicates.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns><see langword="true"/> if a duplicate exists; otherwise <see langword="false"/>.</returns>
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
