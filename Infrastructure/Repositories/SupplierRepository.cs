using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Domain.Entities.Masterlist;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SupplierRepository(AppDbContext context) : ISupplierRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Supplier>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken)
        {
            IQueryable<Supplier> query = _context.Suppliers;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(s => s.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(s =>
                    s.Name.Contains(genericFiltersDTO.SearchTerm)
                );
            }

            if (sort.SortBy != null)
            {
                bool isAsc = string.Equals(sort.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                bool isDsc = string.Equals(sort.SortDirection, "dsc", StringComparison.OrdinalIgnoreCase);

                query = sort.SortBy.ToLower() switch
                {
                    "id" when isAsc => query.OrderBy(s => s.Id),
                    "id" when isDsc => query.OrderByDescending(s => s.Id),
                    "name" when isAsc => query.OrderBy(s => s.Name),
                    "name" when isDsc => query.OrderByDescending(s => s.Name),
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
            IQueryable<Supplier> query = _context.Suppliers;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(s => s.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(s =>
                    s.Name.Contains(genericFiltersDTO.SearchTerm)
                );
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<Supplier?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            IQueryable<Supplier> query = _context.Suppliers
                .Where(s => s.Id == id);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Supplier?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            IQueryable<Supplier> query = _context.Suppliers
                .Where(s => s.Name.ToLower() == name.ToLower());

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken)
        {
            await _context.Suppliers.AddAsync(supplier, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken)
        {
            return await _context.Suppliers.AnyAsync(s => s.Id != id && s.Name.ToLower() == name.ToLower(), cancellationToken);
        }
    }
}
