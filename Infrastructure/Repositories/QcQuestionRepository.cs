using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Domain.Entities.Masterlist;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class QcQuestionRepository : IQcQuestionRepository
    {
        private readonly AppDbContext _context;

        public QcQuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<QcQuestion>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken)
        {
            IQueryable<QcQuestion> query = _context.QcQuestions;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(q => q.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(q => q.Question.Contains(genericFiltersDTO.SearchTerm));
            }

            if (sort.SortBy != null)
            {
                bool isAsc = string.Equals(sort.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                bool isDsc = string.Equals(sort.SortDirection, "dsc", StringComparison.OrdinalIgnoreCase);

                query = sort.SortBy.ToLower() switch
                {
                    "id" when isAsc => query.OrderBy(q => q.Id),
                    "id" when isDsc => query.OrderByDescending(q => q.Id),
                    "order" when isAsc => query.OrderBy(q => q.Order),
                    "order" when isDsc => query.OrderByDescending(q => q.Order),
                    "qcsectionid" when isAsc => query.OrderBy(q => q.QcSectionId),
                    "qcsectionid" when isDsc => query.OrderByDescending(q => q.QcSectionId),
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
            IQueryable<QcQuestion> query = _context.QcQuestions;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(q => q.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(q => q.Question.Contains(genericFiltersDTO.SearchTerm));
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<QcQuestion?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.QcQuestions
                .Where(q => q.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> ExistsInSameOrderAsync(int order, int? id, int? qcSectionId, CancellationToken cancellationToken)
        {
            return await _context.QcQuestions.AnyAsync(q => q.Order == order && q.Id != id && q.QcSectionId == qcSectionId, cancellationToken);
        }

        public async Task AddAsync(QcQuestion qcQuestion, CancellationToken cancellationToken)
        {
            await _context.QcQuestions.AddAsync(qcQuestion, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(QcQuestion qcQuestion, CancellationToken cancellationToken)
        {
            _context.QcQuestions.Update(qcQuestion);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
