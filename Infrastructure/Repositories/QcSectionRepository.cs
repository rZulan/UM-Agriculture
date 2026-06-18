using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Domain.Entities.Masterlist;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class QcSectionRepository(AppDbContext context) : IQcSectionRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<QcSection>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken)
        {
            IQueryable<QcSection> query = _context.QcSections;

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
                    "order" when isAsc => query.OrderBy(q => q.Order),
                    "order" when isDsc => query.OrderByDescending(q => q.Order),
                    "qcformid" when isAsc => query.OrderBy(q => q.QcFormId),
                    "qcformid" when isDsc => query.OrderByDescending(q => q.QcFormId),
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
            IQueryable<QcSection> query = _context.QcSections;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(q => q.IsActive == genericFiltersDTO.IsActive);
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<QcSection?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.QcSections
                .Where(q => q.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> ExistsInSameOrderAsync(int order, int? id, CancellationToken cancellationToken)
        {
            return await _context.QcSections.AnyAsync(q => q.Order == order && q.Id != id, cancellationToken);
        }

        public async Task AddAsync(QcSection qcSection, CancellationToken cancellationToken)
        {
            await _context.QcSections.AddAsync(qcSection, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(QcSection qcSection, CancellationToken cancellationToken)
        {
            _context.QcSections.Update(qcSection);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
