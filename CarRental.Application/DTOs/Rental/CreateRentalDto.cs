namespace CarRental.Application.DTOs.Rental;

public class CreateRentalDto
{
    public int CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
