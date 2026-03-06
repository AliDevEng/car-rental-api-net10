using CarRental.Application.DTOs.Customer;

namespace CarRental.Application.Services.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerDto dto);
    Task<CustomerResponseDto> GetCustomerByIdAsync(int id);
    Task<CustomerResponseDto> UpdateCustomerAsync(int id, UpdateCustomerDto dto);
}
