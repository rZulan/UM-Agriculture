using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Domain.Entities.Masterlist;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class QcTypeRepository : IQcTypeRepository
    {
        private readonly AppDbContext _context;

        public QcTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<QcType>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken)
        {
            IQueryable<QcType> query = _context.QcTypes;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(q => q.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(q => q.Name.Contains(genericFiltersDTO.SearchTerm));
            }

            if (sort.SortBy != null)
            {
                bool isAsc = string.Equals(sort.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                bool isDsc = string.Equals(sort.SortDirection, "dsc", StringComparison.OrdinalIgnoreCase);

                query = sort.SortBy.ToLower() switch
                {
                    "id" when isAsc => query.OrderBy(q => q.Id),
                    "id" when isDsc => query.OrderByDescending(q => q.Id),
                    "name" when isAsc => query.OrderBy(q => q.Name),
                    "name" when isDsc => query.OrderByDescending(q => q.Name),
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
            IQueryable<QcType> query = _context.QcTypes;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(q => q.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(q => q.Name.Contains(genericFiltersDTO.SearchTerm));
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<QcType?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.QcTypes
                .Where(q => q.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
