using Domain.Entities.Masterlist;

namespace Domain.Entities
{
    public class QcAnswer : BaseEntity
    {
        public string? Answer { get; set; }

        public required int QcResponseId { get; set; }
        public required int QcQuestionId { get; set; }

        public QcResponse QcResponse { get; set; } = null!;
        public QcQuestion QcQuestion { get; set; } = null!;
    }
}
