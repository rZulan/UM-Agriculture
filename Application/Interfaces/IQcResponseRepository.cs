using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides data access operations for <see cref="QcResponse"/> entities.
    /// </summary>
    public interface IQcResponseRepository
    {
        /// <summary>Returns a filtered and sorted list of all QC responses for the given QC type.</summary>
        /// <param name="genericFiltersDTO">Search and pagination filters.</param>
        /// <param name="sort">Sort direction and field.</param>
        /// <param name="qcType">The QC type name used to scope the results.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<List<QcResponse>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, string qcType, CancellationToken cancellationToken);

        /// <summary>Returns the total count of QC responses matching the given filters.</summary>
        /// <param name="genericFiltersDTO">Filters to apply before counting.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);

        /// <summary>Returns a QC response by its ID, or <see langword="null"/> if not found.</summary>
        /// <param name="id">The QC response's unique identifier.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<QcResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>Checks whether a QC response already exists for the given dispatch (excluding the given response ID).</summary>
        /// <param name="dispatchId">The dispatch ID to check.</param>
        /// <param name="id">The ID of the response to exclude from the check, or <see langword="null"/> for new responses.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns><see langword="true"/> if a response for the same dispatch already exists; otherwise <see langword="false"/>.</returns>
        Task<bool> ExistsInSameDispatchId(int dispatchId, int? id, CancellationToken cancellationToken);

        /// <summary>Persists a new QC response to the data store.</summary>
        /// <param name="qcResponse">The QC response entity to add.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task AddAsync(QcResponse qcResponse, CancellationToken cancellationToken);

        /// <summary>Saves changes to an existing QC response.</summary>
        /// <param name="qcResponse">The QC response entity with updated values.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task UpdateAsync(QcResponse qcResponse, CancellationToken cancellationToken);
    }
}
