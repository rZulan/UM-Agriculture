using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides data access operations for <see cref="HorticultureClass"/> entities.
    /// </summary>
    public interface IHorticultureClassRepository
    {
        /// <summary>Returns a filtered and sorted list of all horticulture classes.</summary>
        /// <param name="genericFiltersDTO">Search and pagination filters.</param>
        /// <param name="sort">Sort direction and field.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<List<HorticultureClass>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);

        /// <summary>Returns the total count of horticulture classes matching the given filters.</summary>
        /// <param name="genericFiltersDTO">Filters to apply before counting.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);

        /// <summary>Returns a horticulture class by its ID, or <see langword="null"/> if not found.</summary>
        /// <param name="id">The horticulture class's unique identifier.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<HorticultureClass?> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>Returns a horticulture class matching the given name, or <see langword="null"/> if not found.</summary>
        /// <param name="name">The exact horticulture class name to search for.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<HorticultureClass?> GetByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>Persists a new horticulture class to the data store.</summary>
        /// <param name="HorticultureClass">The horticulture class entity to add.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task AddAsync(HorticultureClass HorticultureClass, CancellationToken cancellationToken);

        /// <summary>Saves changes to an existing horticulture class.</summary>
        /// <param name="HorticultureClass">The horticulture class entity with updated values.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task UpdateAsync(HorticultureClass HorticultureClass, CancellationToken cancellationToken);

        /// <summary>Checks whether another horticulture class (excluding the given ID) already uses the specified name.</summary>
        /// <param name="id">The ID of the horticulture class to exclude from the check.</param>
        /// <param name="name">The name to check for duplicates.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns><see langword="true"/> if a duplicate exists; otherwise <see langword="false"/>.</returns>
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
