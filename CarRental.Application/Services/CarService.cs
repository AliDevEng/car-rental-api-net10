using FluentValidation;
using Microsoft.EntityFrameworkCore;
using CarRental.Application.DTOs.Car;
using CarRental.Application.DTOs.Common;
using CarRental.Application.Exceptions;
using CarRental.Application.Services.Interfaces;
using CarRental.Core.Entities;
using CarRental.Infrastructure.Data;

namespace CarRental.Application.Services;

public class CarService : ICarService
{
    private readonly CarRentalDbContext _context;
    private readonly IValidator<CreateCarDto> _createValidator;
    private readonly IValidator<UpdateCarDto> _updateValidator;

    public CarService(
        CarRentalDbContext context,
        IValidator<CreateCarDto> createValidator,
        IValidator<UpdateCarDto> updateValidator)
    {
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<PagedResult<CarResponseDto>> GetAllCarsAsync(CarFilterDto filter)
    {
        var query = _context.Cars
            .Include(c => c.Category)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.Brand))
            query = query.Where(c => c.Brand.Contains(filter.Brand));

        if (filter.CategoryId.HasValue)
            query = query.Where(c => c.CategoryId == filter.CategoryId.Value);

        if (!string.IsNullOrWhiteSpace(filter.Fuel))
            query = query.Where(c => c.Fuel == filter.Fuel);

        if (!string.IsNullOrWhiteSpace(filter.Transmission))
            query = query.Where(c => c.Transmission == filter.Transmission);

        if (filter.MinPrice.HasValue)
            query = query.Where(c => c.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(c => c.Price <= filter.MaxPrice.Value);

        if (!string.IsNullOrWhiteSpace(filter.Status))
            query = query.Where(c => c.Status == filter.Status);

        // Apply sorting
        query = filter.SortBy?.ToLower() switch
        {
            "brand" => filter.SortDirection == "desc"
                ? query.OrderByDescending(c => c.Brand)
                : query.OrderBy(c => c.Brand),
            "price" => filter.SortDirection == "desc"
                ? query.OrderByDescending(c => c.Price)
                : query.OrderBy(c => c.Price),
            "year" => filter.SortDirection == "desc"
                ? query.OrderByDescending(c => c.Year)
                : query.OrderBy(c => c.Year),
            "model" => filter.SortDirection == "desc"
                ? query.OrderByDescending(c => c.Model)
                : query.OrderBy(c => c.Model),
            _ => filter.SortDirection == "desc"
                ? query.OrderByDescending(c => c.Id)
                : query.OrderBy(c => c.Id)
        };

        var totalCount = await query.CountAsync();

        var cars = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(c => MapToResponseDto(c))
            .ToListAsync();

        return new PagedResult<CarResponseDto>
        {
            Items = cars,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<CarResponseDto> GetCarByIdAsync(int id)
    {
        var car = await _context.Cars
            .Include(c => c.Category)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (car == null)
            throw new NotFoundException("Car", id);

        return MapToResponseDto(car);
    }

    public async Task<CarResponseDto> CreateCarAsync(CreateCarDto dto)
    {
        await ValidateAsync(_createValidator, dto);

        // Check if category exists
        var categoryExists = await _context.CarsCategories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!categoryExists)
            throw new NotFoundException("Category", dto.CategoryId);

        // Check for duplicate registration number
        var regNrExists = await _context.Cars.AnyAsync(c => c.RegNr == dto.RegNr);
        if (regNrExists)
            throw new ConflictException($"A car with registration number '{dto.RegNr}' already exists.");

        var car = new Car
        {
            CategoryId = dto.CategoryId,
            Brand = dto.Brand,
            Model = dto.Model,
            Year = dto.Year,
            RegNr = dto.RegNr,
            Fuel = dto.Fuel,
            Transmission = dto.Transmission,
            Seats = dto.Seats,
            Price = dto.Price,
            Status = dto.Status,
            CreatedAt = DateTime.UtcNow
        };

        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

        // Reload with category
        await _context.Entry(car).Reference(c => c.Category).LoadAsync();

        return MapToResponseDto(car);
    }

    public async Task<CarResponseDto> UpdateCarAsync(int id, UpdateCarDto dto)
    {
        await ValidateAsync(_updateValidator, dto);

        var car = await _context.Cars
            .Include(c => c.Category)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (car == null)
            throw new NotFoundException("Car", id);

        // Check if category exists
        var categoryExists = await _context.CarsCategories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!categoryExists)
            throw new NotFoundException("Category", dto.CategoryId);

        // Check for duplicate registration number (exclude current car)
        var regNrExists = await _context.Cars.AnyAsync(c => c.RegNr == dto.RegNr && c.Id != id);
        if (regNrExists)
            throw new ConflictException($"A car with registration number '{dto.RegNr}' already exists.");

        car.CategoryId = dto.CategoryId;
        car.Brand = dto.Brand;
        car.Model = dto.Model;
        car.Year = dto.Year;
        car.RegNr = dto.RegNr;
        car.Fuel = dto.Fuel;
        car.Transmission = dto.Transmission;
        car.Seats = dto.Seats;
        car.Price = dto.Price;
        car.Status = dto.Status;

        await _context.SaveChangesAsync();

        // Reload category if changed
        await _context.Entry(car).Reference(c => c.Category).LoadAsync();

        return MapToResponseDto(car);
    }

    public async Task DeleteCarAsync(int id)
    {
        var car = await _context.Cars.FindAsync(id);

        if (car == null)
            throw new NotFoundException("Car", id);

        // Check if car has active rentals
        var hasActiveRentals = await _context.Rentals
            .AnyAsync(r => r.CarId == id && (r.Status == "ACTIVE" || r.Status == "PENDING"));

        if (hasActiveRentals)
            throw new ConflictException("Cannot delete a car with active or pending rentals.");

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
    }

    private static CarResponseDto MapToResponseDto(Car car)
    {
        return new CarResponseDto
        {
            Id = car.Id,
            CategoryId = car.CategoryId,
            CategoryName = car.Category?.Name ?? string.Empty,
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            RegNr = car.RegNr,
            Fuel = car.Fuel,
            Transmission = car.Transmission,
            Seats = car.Seats,
            Price = car.Price,
            Status = car.Status,
            CreatedAt = car.CreatedAt
        };
    }

    private static async Task ValidateAsync<T>(IValidator<T> validator, T instance)
    {
        var result = await validator.ValidateAsync(instance);
        if (!result.IsValid)
        {
            var errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            throw new AppValidationException(errors);
        }
    }
}
