namespace CarRental.Application.DTOs.Car;

public class CarFilterDto
{
    public string? Brand { get; set; }
    public int? CategoryId { get; set; }
    public string? Fuel { get; set; }
    public string? Transmission { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = "Id";
    public string SortDirection { get; set; } = "asc";
}
