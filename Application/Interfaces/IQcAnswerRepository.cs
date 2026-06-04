using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IQcAnswerRepository
    {
        Task<List<QcAnswer>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken);
        Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken);
        Task<QcAnswer?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(QcAnswer qcAnswer, CancellationToken cancellationToken);
        Task UpdateAsync(QcAnswer qcAnswer, CancellationToken cancellationToken);
        Task<bool> NoAnswerForRequiredQuestion(List<int> questionIds, CancellationToken cancellationToken);
    }
}
