using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Domain.Entities.Masterlist;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly AppDbContext _context;

        public ProductCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductCategory>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken)
        {
            IQueryable<ProductCategory> query = _context.ProductCategories;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(u => u.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(u =>
                    u.Name.Contains(genericFiltersDTO.SearchTerm)
                );
            }

            if (sort.SortBy != null)
            {
                bool isAsc = string.Equals(sort.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                bool isDsc = string.Equals(sort.SortDirection, "dsc", StringComparison.OrdinalIgnoreCase);

                query = sort.SortBy.ToLower() switch
                {
                    "id" when isAsc => query.OrderBy(u => u.Id),
                    "id" when isDsc => query.OrderByDescending(u => u.Id),
                    "name" when isAsc => query.OrderBy(u => u.Name),
                    "name" when isDsc => query.OrderByDescending(u => u.Name),
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
            IQueryable<ProductCategory> query = _context.ProductCategories;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(u => u.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(u =>
                    u.Name.Contains(genericFiltersDTO.SearchTerm)
                );
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<ProductCategory?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            IQueryable<ProductCategory> query = _context.ProductCategories
                .Where(u => u.Id == id);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(ProductCategory productCategory, CancellationToken cancellationToken)
        {
            await _context.ProductCategories.AddAsync(productCategory, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(ProductCategory productCategory, CancellationToken cancellationToken)
        {
            _context.ProductCategories.Update(productCategory);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken)
        {
            return await _context.ProductCategories.AnyAsync(p => p.Id != id && p.Name.ToLower() == name.ToLower(), cancellationToken);
        }
    }
}
