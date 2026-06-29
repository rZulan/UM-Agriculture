using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.QcAnswer;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.QcAnswers.Queries
{
    /// <summary>Query to retrieve a filtered, sorted, and paginated list of QC answers.</summary>
    /// <param name="GenericFiltersDTO">Search and pagination filters.</param>
    /// <param name="Sort">Sort direction and field.</param>
    public record GetQcAnswersQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetQcAnswerDTO>>>;
    public class GetQcAnswersQueryHandler(IQcAnswerRepository qcAnswerRepository) : IRequestHandler<GetQcAnswersQuery, GetAllResult<List<GetQcAnswerDTO>>>
    {
        private readonly IQcAnswerRepository _qcAnswerRepository = qcAnswerRepository;

        public async Task<GetAllResult<List<GetQcAnswerDTO>>> Handle(GetQcAnswersQuery request, CancellationToken cancellationToken)
        {
            var qcAnswers = await _qcAnswerRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = qcAnswers.Select(x => new GetQcAnswerDTO
            {
                Id = x.Id,
                IsActive = x.IsActive,
                Answer = x.Answer,
                QcResponseId = x.QcResponseId,
                QcQuestionId = x.QcQuestionId,
            }).ToList();

            PaginationInfo? paginationInfo = null;

            if (request.GenericFiltersDTO.UsePagination == true)
            {
                paginationInfo = new PaginationInfo
                {
                    CurrentPage = request.GenericFiltersDTO.PageNumber,
                    PageSize = request.GenericFiltersDTO.PageSize,
                    TotalCount = await _qcAnswerRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
                };
            }

            var sortInfo = new SortInfo
            {
                SortColumns = ["id", "qcResponseId", "qcQuestionId"],
                CurrentSort = request.Sort != null ? new CurrentSort
                {
                    Column = request.Sort.SortBy,
                    Direction = request.Sort.SortDirection
                } : null
            };

            return GetAllResult<List<GetQcAnswerDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
