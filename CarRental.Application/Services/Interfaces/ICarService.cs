using CarRental.Application.DTOs.Car;
using CarRental.Application.DTOs.Common;

namespace CarRental.Application.Services.Interfaces;

public interface ICarService
{
    Task<PagedResult<CarResponseDto>> GetAllCarsAsync(CarFilterDto filter);
    Task<CarResponseDto> GetCarByIdAsync(int id);
    Task<CarResponseDto> CreateCarAsync(CreateCarDto dto);
    Task<CarResponseDto> UpdateCarAsync(int id, UpdateCarDto dto);
    Task DeleteCarAsync(int id);
}
