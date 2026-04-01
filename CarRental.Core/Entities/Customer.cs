namespace CarRental.Core.Entities;

public class Customer
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
