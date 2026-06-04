using Application.DTO.Misc;
using Application.DTO.Misc.Sorts;
using Application.DTO.QcQuestion;
using Application.Interfaces;
using Application.Results;
using MediatR;

namespace Application.Features.QcQuestions.Queries
{
    public record GetQcQuestionsQuery(GenericFiltersDTO GenericFiltersDTO, Sort Sort) : IRequest<GetAllResult<List<GetQcQuestionDTO>>>;
    public class GetQcQuestionsQueryHandler(IQcQuestionRepository qcQuestionRepository) : IRequestHandler<GetQcQuestionsQuery, GetAllResult<List<GetQcQuestionDTO>>>
    {
        private readonly IQcQuestionRepository _qcQuestionRepository = qcQuestionRepository;

        public async Task<GetAllResult<List<GetQcQuestionDTO>>> Handle(GetQcQuestionsQuery request, CancellationToken cancellationToken)
        {
            var qcQuestions = await _qcQuestionRepository.GetAllAsync(request.GenericFiltersDTO, request.Sort, cancellationToken);

            var result = qcQuestions.Select(x => new GetQcQuestionDTO
            {
                Id = x.Id,
                IsActive = x.IsActive,
                Question = x.Question,
                IsRequired = x.IsRequired,
                CorrectAnswer = x.CorrectAnswer,
                Order = x.Order,
                QcSectionId = x.QcSectionId,
                QcAnswerTypeId = x.QcAnswerTypeId,
            }).ToList();

            PaginationInfo? paginationInfo = null;

            if (request.GenericFiltersDTO.UsePagination == true)
            {
                paginationInfo = new PaginationInfo
                {
                    CurrentPage = request.GenericFiltersDTO.PageNumber,
                    PageSize = request.GenericFiltersDTO.PageSize,
                    TotalCount = await _qcQuestionRepository.GetCountAsync(request.GenericFiltersDTO, cancellationToken)
                };
            }

            var sortInfo = new SortInfo
            {
                SortColumns = ["id", "order", "qcSectionId"],
                CurrentSort = request.Sort != null ? new CurrentSort
                {
                    Column = request.Sort.SortBy,
                    Direction = request.Sort.SortDirection
                } : null
            };

            return GetAllResult<List<GetQcQuestionDTO>>.Success(result, paginationInfo: paginationInfo, sortInfo: sortInfo);
        }
    }
}
