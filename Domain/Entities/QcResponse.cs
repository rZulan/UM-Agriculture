using Domain.Entities.Masterlist;
using Domain.Entities.Sync;

namespace Domain.Entities
{
    public class QcResponse : BaseEntity
    {
        public required int QcFormId { get; set; }
        public required int ResponderId { get; set; }
        public int? DispatchId { get; set; }
        public int? OutsourceId { get; set; }
        public int? PurchaseOrderId { get; set; }

        public QcForm QcForm { get; set; } = null!;
        public User Responder { get; set; } = null!;
        public Dispatch? Dispatch { get; set; }
        public Outsource? Outsource { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
        public List<QcAnswer> QcAnswers { get; set; } = [];
    }
}
