using CarRental.Application.DTOs.Admin;
using CarRental.Application.DTOs.Customer;

namespace CarRental.Application.Services.Interfaces;

public interface IAdminService
{
    Task<DashboardStatsDto> GetDashboardStatsAsync();
    Task<List<CustomerResponseDto>> GetAllCustomersAsync();
}
