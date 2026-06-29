using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides data access operations for <see cref="Category"/> entities.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>Returns a filtered and sorted list of all categories.</summary>
        /// <param name="genericFiltersDTO">Search and pagination filters.</param>
        /// <param name="sort">Sort direction and field.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<List<Category>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        /// <summary>Returns the total count of categories matching the given filters.</summary>
        /// <param name="genericFiltersDTO">Filters to apply before counting.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        /// <summary>Returns a category by its ID, or <see langword="null"/> if not found.</summary>
        /// <param name="id">The category's unique identifier.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken);
        /// <summary>Returns a category matching the given name, or <see langword="null"/> if not found.</summary>
        /// <param name="name">The exact category name to search for.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken);
        /// <summary>Persists a new category to the data store.</summary>
        /// <param name="category">The category entity to add.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task AddAsync(Category category, CancellationToken cancellationToken);
        /// <summary>Saves changes to an existing category.</summary>
        /// <param name="category">The category entity with updated values.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task UpdateAsync(Category category, CancellationToken cancellationToken);
        /// <summary>Checks whether another category (excluding the given ID) already uses the specified name.</summary>
        /// <param name="id">The ID of the category to exclude from the check.</param>
        /// <param name="name">The name to check for duplicates.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns><see langword="true"/> if a duplicate exists; otherwise <see langword="false"/>.</returns>
        Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken);
    }
}
