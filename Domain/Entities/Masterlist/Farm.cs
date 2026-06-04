namespace Domain.Entities.Masterlist
{
    public class Farm : BaseEntity
    {
        public required string Name { get; set; }
        public string? Address { get; set; }
    }
}
