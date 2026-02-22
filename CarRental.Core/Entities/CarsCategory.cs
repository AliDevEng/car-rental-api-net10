namespace CarRental.Core.Entities;

public class CarsCategory
{
    public int Id { get; set; }
    public required string Name { get; set; }

    // Navigation property
    public ICollection<Car> Cars { get; set; } = new List<Car>();
}
