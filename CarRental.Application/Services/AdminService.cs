using Microsoft.EntityFrameworkCore;
using CarRental.Application.DTOs.Admin;
using CarRental.Application.DTOs.Customer;
using CarRental.Application.Services.Interfaces;
using CarRental.Infrastructure.Data;

namespace CarRental.Application.Services;

public class AdminService : IAdminService
{
    private readonly CarRentalDbContext _context;

    public AdminService(CarRentalDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        return new DashboardStatsDto
        {
            TotalCars = await _context.Cars.CountAsync(),
            AvailableCars = await _context.Cars.CountAsync(c => c.Status == "Available"),
            RentedCars = await _context.Cars.CountAsync(c => c.Status == "Rented"),
            TotalCustomers = await _context.Customers.CountAsync(),
            TotalRentals = await _context.Rentals.CountAsync(),
            ActiveRentals = await _context.Rentals.CountAsync(r => r.Status == "ACTIVE"),
            TotalCategories = await _context.CarsCategories.CountAsync()
        };
    }

    public async Task<List<CustomerResponseDto>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CustomerResponseDto
            {
                Id = c.Id,
                Email = c.Email,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Phone = c.Phone,
                Address = c.Address,
                City = c.City,
                PostalCode = c.PostalCode,
                Country = c.Country,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();
    }
}
