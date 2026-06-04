using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Domain.Entities.Masterlist;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FarmRepository : IFarmRepository
    {
        private readonly AppDbContext _context;

        public FarmRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Farm>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken)
        {
            IQueryable<Farm> query = _context.Farms;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(f => f.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(f =>
                    f.Name.Contains(genericFiltersDTO.SearchTerm)
                );
            }

            if (sort.SortBy != null)
            {
                bool isAsc = string.Equals(sort.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                bool isDsc = string.Equals(sort.SortDirection, "dsc", StringComparison.OrdinalIgnoreCase);

                query = sort.SortBy.ToLower() switch
                {
                    "id" when isAsc => query.OrderBy(f => f.Id),
                    "id" when isDsc => query.OrderByDescending(f => f.Id),
                    "name" when isAsc => query.OrderBy(f => f.Name),
                    "name" when isDsc => query.OrderByDescending(f => f.Name),
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
            IQueryable<Farm> query = _context.Farms;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(f => f.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(f =>
                    f.Name.Contains(genericFiltersDTO.SearchTerm)
                );
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<Farm?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            IQueryable<Farm> query = _context.Farms
                .Where(f => f.Id == id);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Farm?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            IQueryable<Farm> query = _context.Farms
                .Where(f => f.Name.ToLower() == name.ToLower());

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(Farm farm, CancellationToken cancellationToken)
        {
            await _context.Farms.AddAsync(farm, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Farm farm, CancellationToken cancellationToken)
        {
            _context.Farms.Update(farm);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken)
        {
            return await _context.Farms.AnyAsync(f => f.Id != id && f.Name.ToLower() == name.ToLower(), cancellationToken);
        }
    }
}
