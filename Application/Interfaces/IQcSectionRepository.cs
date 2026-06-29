using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides data access operations for <see cref="QcSection"/> entities.
    /// </summary>
    public interface IQcSectionRepository
    {
        /// <summary>Returns a filtered and sorted list of all QC sections.</summary>
        /// <param name="genericFiltersDTO">Search and pagination filters.</param>
        /// <param name="sort">Sort direction and field.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<List<QcSection>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        /// <summary>Returns the total count of QC sections matching the given filters.</summary>
        /// <param name="genericFiltersDTO">Filters to apply before counting.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        /// <summary>Returns a QC section by its ID, or <see langword="null"/> if not found.</summary>
        /// <param name="id">The QC section's unique identifier.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<QcSection?> GetByIdAsync(int id, CancellationToken cancellationToken);
        /// <summary>Checks whether a QC section already exists at the given order position (excluding the given section ID).</summary>
        /// <param name="order">The display order to check.</param>
        /// <param name="id">The ID of the section to exclude from the check, or <see langword="null"/> for new sections.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns><see langword="true"/> if the order position is already taken; otherwise <see langword="false"/>.</returns>
        Task<bool> ExistsInSameOrderAsync(int order, int? id, CancellationToken cancellationToken);
        /// <summary>Persists a new QC section to the data store.</summary>
        /// <param name="qcSection">The QC section entity to add.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task AddAsync(QcSection qcSection, CancellationToken cancellationToken);
        /// <summary>Saves changes to an existing QC section.</summary>
        /// <param name="qcSection">The QC section entity with updated values.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task UpdateAsync(QcSection qcSection, CancellationToken cancellationToken);
    }
}
