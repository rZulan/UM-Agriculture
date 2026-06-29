namespace Domain.Entities.Masterlist
{
    public class QcQuestion : BaseEntity
    {
        public required string Question { get; set; }
        public required bool IsRequired { get; set; }
        public string? CorrectAnswer { get; set; }
        public required int Order { get; set; }

        public int QcSectionId { get; set; }
        public required int QcAnswerTypeId { get; set; }

        public QcSection QcSection { get; set; } = null!;
        public QcAnswerType QcAnswerType { get; set; } = null!;
        public List<QcAnswer> QcAnswers { get; set; } = [];
    }
}
