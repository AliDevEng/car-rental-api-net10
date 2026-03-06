namespace CarRental.Application.DTOs.Car;

public class CreateCarDto
{
    public int CategoryId { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string RegNr { get; set; } = string.Empty;
    public string Fuel { get; set; } = string.Empty;
    public string Transmission { get; set; } = string.Empty;
    public int Seats { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; } = "Available";
}
