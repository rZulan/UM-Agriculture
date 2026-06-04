using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities.Masterlist;

namespace Application.Interfaces
{
    public interface IQcQuestionRepository
    {
        Task<List<QcQuestion>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<QcQuestion?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> ExistsInSameOrderAsync(int order, int? id, int? qcSectionId, CancellationToken cancellationToken);
        Task AddAsync(QcQuestion qcQuestion, CancellationToken cancellationToken);
        Task UpdateAsync(QcQuestion qcQuestion, CancellationToken cancellationToken);
    }
}
