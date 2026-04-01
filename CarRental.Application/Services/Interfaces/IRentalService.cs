using CarRental.Application.DTOs.Rental;
using CarRental.Application.DTOs.Common;

namespace CarRental.Application.Services.Interfaces;

public interface IRentalService
{
    Task<RentalResponseDto> CreateRentalAsync(int customerId, CreateRentalDto dto);
    Task<RentalResponseDto> GetRentalByIdAsync(int rentalId, int requesterId, bool isAdmin);
    Task<PagedResult<RentalResponseDto>> GetCustomerRentalsAsync(int customerId, int page = 1, int pageSize = 10);
    Task<PagedResult<RentalResponseDto>> GetAllRentalsAsync(int page = 1, int pageSize = 10);
    Task<RentalResponseDto> UpdateRentalStatusAsync(int rentalId, string status);
    Task CancelRentalAsync(int rentalId, int customerId);
    Task<bool> IsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate);
}
