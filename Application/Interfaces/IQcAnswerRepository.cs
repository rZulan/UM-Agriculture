using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides data access operations for <see cref="QcAnswer"/> entities.
    /// </summary>
    public interface IQcAnswerRepository
    {
        /// <summary>Returns a filtered and sorted list of all QC answers.</summary>
        /// <param name="genericFiltersDTO">Search and pagination filters.</param>
        /// <param name="sort">Sort direction and field.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<List<QcAnswer>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        /// <summary>Returns the total count of QC answers matching the given filters.</summary>
        /// <param name="genericFiltersDTO">Filters to apply before counting.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        /// <summary>Returns a QC answer by its ID, or <see langword="null"/> if not found.</summary>
        /// <param name="id">The QC answer's unique identifier.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<QcAnswer?> GetByIdAsync(int id, CancellationToken cancellationToken);
        /// <summary>Persists a new QC answer to the data store.</summary>
        /// <param name="qcAnswer">The QC answer entity to add.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task AddAsync(QcAnswer qcAnswer, CancellationToken cancellationToken);
        /// <summary>Saves changes to an existing QC answer.</summary>
        /// <param name="qcAnswer">The QC answer entity with updated values.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task UpdateAsync(QcAnswer qcAnswer, CancellationToken cancellationToken);
        /// <summary>Checks whether any of the given required questions have no answer submitted.</summary>
        /// <param name="questionIds">The list of required question IDs to validate.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns><see langword="true"/> if at least one required question has no answer; otherwise <see langword="false"/>.</returns>
        Task<bool> NoAnswerForRequiredQuestion(List<int> questionIds, CancellationToken cancellationToken);
    }
}
