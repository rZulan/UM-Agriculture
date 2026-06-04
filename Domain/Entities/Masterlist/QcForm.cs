namespace Domain.Entities.Masterlist
{
    public class QcForm : BaseEntity
    {
        public required int QcCategoryId { get; set; }
        public required int QcTypeId { get; set; }

        public QcCategory QcCategory { get; set; } = null!;
        public QcType QcType { get; set; } = null!;
        public List<QcSection> QcSections { get; set; } = [];
        public List<QcResponse> QcResponses { get; set; } = [];
    }
}
