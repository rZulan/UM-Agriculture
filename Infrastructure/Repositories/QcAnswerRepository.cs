using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class QcAnswerRepository : IQcAnswerRepository
    {
        private readonly AppDbContext _context;

        public QcAnswerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<QcAnswer>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken)
        {
            IQueryable<QcAnswer> query = _context.QcAnswers;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(q => q.IsActive == genericFiltersDTO.IsActive);
            }

            if (sort.SortBy != null)
            {
                bool isAsc = string.Equals(sort.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                bool isDsc = string.Equals(sort.SortDirection, "dsc", StringComparison.OrdinalIgnoreCase);

                query = sort.SortBy.ToLower() switch
                {
                    "id" when isAsc => query.OrderBy(q => q.Id),
                    "id" when isDsc => query.OrderByDescending(q => q.Id),
                    "qcresponseid" when isAsc => query.OrderBy(q => q.QcResponseId),
                    "qcresponseid" when isDsc => query.OrderByDescending(q => q.QcResponseId),
                    "qcquestionid" when isAsc => query.OrderBy(q => q.QcQuestionId),
                    "qcquestionid" when isDsc => query.OrderByDescending(q => q.QcQuestionId),
                    _ => query
                };
            }

            if (genericFiltersDTO.UsePagination)
            {
                query = query.Skip((genericFiltersDTO.PageNumber - 1) * genericFiltersDTO.PageSize)
                             .Take(genericFiltersDTO.PageSize);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync(GenericFiltersDTO genericFiltersDTO, CancellationToken cancellationToken)
        {
            IQueryable<QcAnswer> query = _context.QcAnswers;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(q => q.IsActive == genericFiltersDTO.IsActive);
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<QcAnswer?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.QcAnswers
                .Where(q => q.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(QcAnswer qcAnswer, CancellationToken cancellationToken)
        {
            await _context.QcAnswers.AddAsync(qcAnswer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(QcAnswer qcAnswer, CancellationToken cancellationToken)
        {
            _context.QcAnswers.Update(qcAnswer);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> NoAnswerForRequiredQuestion(List<int> questionIds, CancellationToken cancellationToken)
        {
            var requiredQuestionIds = await _context.QcQuestions
                .Where(q => questionIds.Contains(q.Id) && q.IsRequired)
                .Select(q => q.Id)
                .AnyAsync(cancellationToken);

            return requiredQuestionIds;
        }
    }
}
