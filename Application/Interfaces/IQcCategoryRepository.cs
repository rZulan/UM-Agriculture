using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides data access operations for <see cref="QcCategory"/> entities.
    /// </summary>
    public interface IQcCategoryRepository
    {
        /// <summary>Returns a filtered and sorted list of all QC categories.</summary>
        /// <param name="genericFiltersDTO">Search and pagination filters.</param>
        /// <param name="sort">Sort direction and field.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<List<QcCategory>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        /// <summary>Returns the total count of QC categories matching the given filters.</summary>
        /// <param name="genericFiltersDTO">Filters to apply before counting.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        /// <summary>Returns a QC category by its ID, or <see langword="null"/> if not found.</summary>
        /// <param name="id">The QC category's unique identifier.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<QcCategory?> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}
