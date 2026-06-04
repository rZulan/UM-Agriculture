using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.QcResponse;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.QcResponses.Queries
{
    public record GetDispatchQcResponsesQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetDispatchQcResponseDTO>>>;
    public class GetDispatchQcResponsesQueryHandler(IQcResponseRepository qcResponseRepository) : IRequestHandler<GetDispatchQcResponsesQuery, GetAllResult<List<GetDispatchQcResponseDTO>>>
    {
        private readonly IQcResponseRepository _qcResponseRepository = qcResponseRepository;

        public async Task<GetAllResult<List<GetDispatchQcResponseDTO>>> Handle(GetDispatchQcResponsesQuery request, CancellationToken cancellationToken)
        {
            var qcResponses = await _qcResponseRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, "dispatch", cancellationToken);

            var result = qcResponses.Select(qcResponse => new GetDispatchQcResponseDTO
            {
                Id = qcResponse.Id,
                IsActive = qcResponse.IsActive,
                FormName = qcResponse.QcForm.QcCategory.Name + " - " + qcResponse.QcForm.QcType.Name + " QC Form",
                DispatchId = qcResponse.DispatchId,
                BatchNumber = qcResponse.Dispatch!.BatchNumber,
                ItemCode = qcResponse.Dispatch.Product.ItemCode,
                Description = qcResponse.Dispatch.Product.Description,
                Uom = qcResponse.Dispatch.Product.Uom.ShortName,
                QuantityOut = qcResponse.Dispatch.QuantityOut,
                QuantityReturn = qcResponse.Dispatch.QuantityReturn,
                HarvestDate = qcResponse.Dispatch.HarvestDate,
                Sections = [.. qcResponse.QcForm.QcSections
                    .Select(section => new GetQcSectionForResponseDTO
                    {
                        Name = section.Name,
                        Description = section.Description,
                        Questions = [.. (section.QcQuestions ?? [])
                            .Select(q => new GetQcQuestionForResponseDTO
                            {
                                Question = q.Question,
                                Answer = qcResponse.QcAnswers?.FirstOrDefault(a => a.QcQuestionId == q.Id)?.Answer
                            })]
                    })]
            }).ToList();

            PaginationInfo? paginationInfo = null;

            if (request.GenericFiltersDTO.UsePagination == true)
            {
                paginationInfo = new PaginationInfo
                {
                    CurrentPage = request.GenericFiltersDTO.PageNumber,
                    PageSize = request.GenericFiltersDTO.PageSize,
                    TotalCount = await _qcResponseRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
                };
            }

            var sortInfo = new SortInfo
            {
                SortColumns = ["id", "qcFormId", "responderId"],
                CurrentSort = request.Sort != null ? new CurrentSort
                {
                    Column = request.Sort.SortBy,
                    Direction = request.Sort.SortDirection
                } : null
            };

            return GetAllResult<List<GetDispatchQcResponseDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
