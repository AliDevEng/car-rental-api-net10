namespace CarRental.Core.Entities;

public class Payment
{
    public int Id { get; set; }
    public int RentalId { get; set; }
    public decimal Amount { get; set; }
    public required string Method { get; set; }
    public required string Status { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? TransactionId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation property - will be implemented in later iterations
}
