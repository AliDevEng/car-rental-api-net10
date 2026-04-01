namespace CarRental.Core.Entities;

public class Rental
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int CarId { get; set; }
    public int? PaymentId { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public required string BookingNumber { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public Car Car { get; set; } = null!;
    public Payment? Payment { get; set; }
}
