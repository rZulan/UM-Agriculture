using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides data access operations for <see cref="QcQuestion"/> entities.
    /// </summary>
    public interface IQcQuestionRepository
    {
        /// <summary>Returns a filtered and sorted list of all QC questions.</summary>
        /// <param name="genericFiltersDTO">Search and pagination filters.</param>
        /// <param name="sort">Sort direction and field.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<List<QcQuestion>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        /// <summary>Returns the total count of QC questions matching the given filters.</summary>
        /// <param name="genericFiltersDTO">Filters to apply before counting.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        /// <summary>Returns a QC question by its ID, or <see langword="null"/> if not found.</summary>
        /// <param name="id">The QC question's unique identifier.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<QcQuestion?> GetByIdAsync(int id, CancellationToken cancellationToken);
        /// <summary>Checks whether a QC question already exists at the given order position within the same section (excluding the given question ID).</summary>
        /// <param name="order">The display order to check.</param>
        /// <param name="id">The ID of the question to exclude from the check, or <see langword="null"/> for new questions.</param>
        /// <param name="qcSectionId">The section ID to scope the check to, or <see langword="null"/> if not scoped.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns><see langword="true"/> if the order position is already taken; otherwise <see langword="false"/>.</returns>
        Task<bool> ExistsInSameOrderAsync(int order, int? id, int? qcSectionId, CancellationToken cancellationToken);
        /// <summary>Persists a new QC question to the data store.</summary>
        /// <param name="qcQuestion">The QC question entity to add.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task AddAsync(QcQuestion qcQuestion, CancellationToken cancellationToken);
        /// <summary>Saves changes to an existing QC question.</summary>
        /// <param name="qcQuestion">The QC question entity with updated values.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task UpdateAsync(QcQuestion qcQuestion, CancellationToken cancellationToken);
    }
}
