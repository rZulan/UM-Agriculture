namespace Domain.Entities.Masterlist
{
    public class QcCategory : BaseEntity
    {
        public required string Name { get; set; }

        public List<QcForm> QcForms { get; set; } = [];
    }
}
