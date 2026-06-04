namespace Domain.Entities.Masterlist
{
    public class QcSection : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required int Order { get; set; }
        public required int QcFormId { get; set; }

        public QcForm QcForm { get; set; } = null!;
        public List<QcQuestion> QcQuestions { get; set; } = [];
    }
}
