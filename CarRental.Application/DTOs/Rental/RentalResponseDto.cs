namespace CarRental.Application.DTOs.Rental;

public class RentalResponseDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int CarId { get; set; }
    public int? PaymentId { get; set; }
    public string BookingNumber { get; set; } = null!;
    public DateTime RentalDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public required string Status { get; set; }
    public string? CarBrand { get; set; }
    public string? CarModel { get; set; }
    public int CarYear { get; set; }
    public decimal DailyPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
