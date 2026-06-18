using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Domain.Entities.Masterlist;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class HorticultureClassRepository(AppDbContext context) : IHorticultureClassRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<HorticultureClass>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken)
        {
            IQueryable<HorticultureClass> query = _context.HorticultureClasses;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(hc => hc.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(hc =>
                    hc.Name.Contains(genericFiltersDTO.SearchTerm)
                );
            }

            if (sort.SortBy != null)
            {
                bool isAsc = string.Equals(sort.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                bool isDsc = string.Equals(sort.SortDirection, "dsc", StringComparison.OrdinalIgnoreCase);

                query = sort.SortBy.ToLower() switch
                {
                    "id" when isAsc => query.OrderBy(hc => hc.Id),
                    "id" when isDsc => query.OrderByDescending(hc => hc.Id),
                    "name" when isAsc => query.OrderBy(hc => hc.Name),
                    "name" when isDsc => query.OrderByDescending(hc => hc.Name),
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
            IQueryable<HorticultureClass> query = _context.HorticultureClasses;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(hc => hc.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(hc =>
                    hc.Name.Contains(genericFiltersDTO.SearchTerm)
                );
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<HorticultureClass?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            IQueryable<HorticultureClass> query = _context.HorticultureClasses
                .Where(hc => hc.Id == id);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<HorticultureClass?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            IQueryable<HorticultureClass> query = _context.HorticultureClasses
                .Where(hc => hc.Name.ToLower() == name.ToLower());

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(HorticultureClass horticultureClass, CancellationToken cancellationToken)
        {
            await _context.HorticultureClasses.AddAsync(horticultureClass, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(HorticultureClass horticultureClass, CancellationToken cancellationToken)
        {
            _context.HorticultureClasses.Update(horticultureClass);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken)
        {
            return await _context.HorticultureClasses.AnyAsync(hc => hc.Id != id && hc.Name.ToLower() == name.ToLower(), cancellationToken);
        }
    }
}
