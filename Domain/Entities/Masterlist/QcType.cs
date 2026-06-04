namespace Domain.Entities.Masterlist
{
    public class QcType : BaseEntity
    {
        public required string Name { get; set; } // Dispatch, Outside Purchase, Import PO

        public List<QcForm> QcForms { get; set; } = [];
    }
}
