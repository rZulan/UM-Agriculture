using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class QcResponseRepository : IQcResponseRepository
    {
        private readonly AppDbContext _context;

        public QcResponseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<QcResponse>> GetAllAsync(GenericFiltersDTO genericFiltersDTO, Sort sort, string qcType, CancellationToken cancellationToken)
        {
            IQueryable<QcResponse> query = _context.QcResponses
                .Include(x => x.QcAnswers)
                .Include(x => x.QcForm)
                    .ThenInclude(x => x.QcCategory)
                .Include(x => x.QcForm)
                    .ThenInclude(x => x.QcType)
                .Include(x => x.QcForm)
                    .ThenInclude(x => x.QcSections)
                        .ThenInclude(x => x.QcQuestions)
                .Include(x => x.Dispatch)
                    .ThenInclude(x => x!.Product)
                        .ThenInclude(x => x!.Uom);

            if (qcType == "dispatch")
            {
                query = query.Where(q => q.DispatchId != null);
            }
            else if (qcType == "outsource")
            {
                query = query.Where(q => q.OutsourceId != null);
            }
            else if (qcType == "purchaseorder")
            {
                query = query.Where(q => q.PurchaseOrderId != null);
            }

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
                    "qcformid" when isAsc => query.OrderBy(q => q.QcFormId),
                    "qcformid" when isDsc => query.OrderByDescending(q => q.QcFormId),
                    "responderid" when isAsc => query.OrderBy(q => q.ResponderId),
                    "responderid" when isDsc => query.OrderByDescending(q => q.ResponderId),
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
            IQueryable<QcResponse> query = _context.QcResponses;

            if (genericFiltersDTO.IsActive != null)
            {
                query = query.Where(q => q.IsActive == genericFiltersDTO.IsActive);
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<QcResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.QcResponses
                .Where(q => q.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> ExistsInSameDispatchId(int dispatchId, int? id, CancellationToken cancellationToken)
        {
            return await _context.QcResponses.AnyAsync(q => q.DispatchId == dispatchId && q.Id != id, cancellationToken);
        }

        public async Task AddAsync(QcResponse qcResponse, CancellationToken cancellationToken)
        {
            await _context.QcResponses.AddAsync(qcResponse, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(QcResponse qcResponse, CancellationToken cancellationToken)
        {
            _context.QcResponses.Update(qcResponse);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
