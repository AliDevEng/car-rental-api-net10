namespace CarRental.Application.DTOs.Admin;

public class DashboardStatsDto
{
    public int TotalCars { get; set; }
    public int AvailableCars { get; set; }
    public int RentedCars { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalRentals { get; set; }
    public int ActiveRentals { get; set; }
    public int TotalCategories { get; set; }
}
