using Application.DTO.QcQuestion;
using Application.Interfaces;
using Application.Results;
using Domain.Entities.Masterlist;
using MediatR;
using System.Net;

namespace Application.Features.QcQuestions.Commands
{
    /// <summary>Command to add a new question to a QC section.</summary>
    /// <param name="UserId">The ID of the authenticated user performing the action.</param>
    /// <param name="AddQcQuestionDTO">The QC question data to be created.</param>
    public record AddQcQuestionCommand(int? UserId, AddQcQuestionDTO AddQcQuestionDTO) : IRequest<Result<object>>;
    public class AddQcQuestionCommandHandler(IQcQuestionRepository qcQuestionRepository, IUserRepository userRepository) : IRequestHandler<AddQcQuestionCommand, Result<object>>
    {
        private readonly IQcQuestionRepository _qcQuestionRepository = qcQuestionRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<object>> Handle(AddQcQuestionCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == null)
            {
                return Result<object>.Failure("User is not signed in", HttpStatusCode.Unauthorized);
            }

            var existingUser = await _userRepository.GetByIdAsync(request.UserId.Value, cancellationToken);

            if (existingUser == null)
            {
                return Result<object>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var existingQuestion = await _qcQuestionRepository.ExistsInSameOrderAsync(request.AddQcQuestionDTO.Order, null, request.AddQcQuestionDTO.QcSectionId, cancellationToken);

            if (existingQuestion)
            {
                return Result<object>.Failure("A question with the same order already exists", HttpStatusCode.Conflict);
            }

            var qcQuestion = new QcQuestion
            {
                Question = request.AddQcQuestionDTO.Question,
                IsRequired = request.AddQcQuestionDTO.IsRequired,
                CorrectAnswer = request.AddQcQuestionDTO.CorrectAnswer,
                Order = request.AddQcQuestionDTO.Order,
                QcSectionId = request.AddQcQuestionDTO.QcSectionId,
                QcAnswerTypeId = request.AddQcQuestionDTO.QcAnswerTypeId,
                CreatedAt = DateTime.UtcNow,
                CreatedById = existingUser.Id
            };

            await _qcQuestionRepository.AddAsync(qcQuestion, cancellationToken);

            return Result<object>.Success(qcQuestion.Id, "QcQuestion created successfully", HttpStatusCode.Created);
        }
    }
}
