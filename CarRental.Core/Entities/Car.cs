namespace CarRental.Core.Entities;

public class Car
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public required string Brand { get; set; }
    public required string Model { get; set; }
    public int Year { get; set; }
    public required string RegNr { get; set; }
    public required string Fuel { get; set; }
    public required string Transmission { get; set; }
    public int Seats { get; set; }
    public decimal Price { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public CarsCategory Category { get; set; } = null!;
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
