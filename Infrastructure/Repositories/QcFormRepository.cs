using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Domain.Entities.Masterlist;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class QcFormRepository(AppDbContext context) : IQcFormRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<QcForm>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken)
        {
            IQueryable<QcForm> query = _context.QcForms
                .Include(x => x.QcSections)
                    .ThenInclude(x => x.QcQuestions)
                .Include(x => x.QcResponses)
                    .ThenInclude(x => x.QcAnswers);

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
                    "qccategoryid" when isAsc => query.OrderBy(q => q.QcCategoryId),
                    "qccategoryid" when isDsc => query.OrderByDescending(q => q.QcCategoryId),
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
            IQueryable<QcForm> query = _context.QcForms;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(q => q.IsActive == genericFiltersDTO.IsActive);
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<QcForm?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.QcForms
                .Include(x => x.QcSections)
                    .ThenInclude(x => x.QcQuestions)
                .Include(x => x.QcResponses)
                    .ThenInclude(x => x.QcAnswers)
                .Where(q => q.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateAsync(QcForm qcForm, CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
