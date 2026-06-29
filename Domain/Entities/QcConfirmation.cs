namespace Domain.Entities
{
    public class QcConfirmation : BaseEntity
    {
        public Guid ConfirmationId { get; set; }
        public int FormId { get; set; }
        public required string Data { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
