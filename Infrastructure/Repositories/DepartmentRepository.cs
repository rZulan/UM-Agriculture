using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Domain.Entities.Masterlist;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DepartmentRepository(AppDbContext context) : IDepartmentRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Department>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, CancellationToken cancellationToken)
        {
            IQueryable<Department> query = _context.Departments;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(d => d.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(d =>
                    d.Name.Contains(genericFiltersDTO.SearchTerm)
                );
            }

            if (sort.SortBy != null)
            {
                bool isAsc = string.Equals(sort.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                bool isDsc = string.Equals(sort.SortDirection, "dsc", StringComparison.OrdinalIgnoreCase);

                query = sort.SortBy.ToLower() switch
                {
                    "id" when isAsc => query.OrderBy(d => d.Id),
                    "id" when isDsc => query.OrderByDescending(d => d.Id),
                    "name" when isAsc => query.OrderBy(d => d.Name),
                    "name" when isDsc => query.OrderByDescending(d => d.Name),
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
            IQueryable<Department> query = _context.Departments;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(d => d.IsActive == genericFiltersDTO.IsActive);
            }

            if (!string.IsNullOrEmpty(genericFiltersDTO.SearchTerm))
            {
                query = query.Where(d =>
                    d.Name.Contains(genericFiltersDTO.SearchTerm)
                );
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            IQueryable<Department> query = _context.Departments
                .Where(d => d.Id == id);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            IQueryable<Department> query = _context.Departments
                .Where(d => d.Name.ToLower() == name.ToLower());

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(Department department, CancellationToken cancellationToken)
        {
            await _context.Departments.AddAsync(department, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Department department, CancellationToken cancellationToken)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckDuplicateAsync(int id, string name, CancellationToken cancellationToken)
        {
            return await _context.Departments.AnyAsync(d => d.Id != id && d.Name.ToLower() == name.ToLower(), cancellationToken);
        }
    }
}
