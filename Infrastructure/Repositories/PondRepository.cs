using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Domain.Entities.Masterlist;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PondRepository : IPondRepository
    {
        private readonly AppDbContext _context;

        public PondRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pond>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken)
        {
            IQueryable<Pond> query = _context.Ponds;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(p => p.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(p =>
                    p.Name.Contains(genericFiltersDTO.SearchTerm)
                );
            }

            if (sort.SortBy != null)
            {
                bool isAsc = string.Equals(sort.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                bool isDsc = string.Equals(sort.SortDirection, "dsc", StringComparison.OrdinalIgnoreCase);

                query = sort.SortBy.ToLower() switch
                {
                    "id" when isAsc => query.OrderBy(p => p.Id),
                    "id" when isDsc => query.OrderByDescending(p => p.Id),
                    "name" when isAsc => query.OrderBy(p => p.Name),
                    "name" when isDsc => query.OrderByDescending(p => p.Name),
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
            IQueryable<Pond> query = _context.Ponds;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(p => p.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(p =>
                    p.Name.Contains(genericFiltersDTO.SearchTerm)
                );
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<Pond?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            IQueryable<Pond> query = _context.Ponds
                .Where(p => p.Id == id);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Pond?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            IQueryable<Pond> query = _context.Ponds
                .Where(p => p.Name.ToLower() == name.ToLower());

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(Pond pond, CancellationToken cancellationToken)
        {
            await _context.Ponds.AddAsync(pond, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Pond pond, CancellationToken cancellationToken)
        {
            _context.Ponds.Update(pond);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken)
        {
            return await _context.Ponds.AnyAsync(p => p.Id != id && p.Name.ToLower() == name.ToLower(), cancellationToken);
        }
    }
}
