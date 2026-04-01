using System.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using CarRental.Application.DTOs.Rental;
using CarRental.Application.DTOs.Common;
using CarRental.Application.Exceptions;
using CarRental.Application.Services.Interfaces;
using CarRental.Core.Entities;
using CarRental.Infrastructure.Data;

namespace CarRental.Application.Services;

public class RentalService : IRentalService
{
    private readonly CarRentalDbContext _context;
    private readonly IValidator<CreateRentalDto> _createRentalValidator;
    private readonly IValidator<UpdateRentalStatusDto> _updateStatusValidator;

    public RentalService(
        CarRentalDbContext context,
        IValidator<CreateRentalDto> createRentalValidator,
        IValidator<UpdateRentalStatusDto> updateStatusValidator)
    {
        _context = context;
        _createRentalValidator = createRentalValidator;
        _updateStatusValidator = updateStatusValidator;
    }

    public async Task<RentalResponseDto> CreateRentalAsync(int customerId, CreateRentalDto dto)
    {
        await ValidateAsync(_createRentalValidator, dto);

        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
            throw new NotFoundException("Customer", customerId);

        var car = await _context.Cars.FindAsync(dto.CarId);
        if (car == null)
            throw new NotFoundException("Car", dto.CarId);

        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var isAvailable = await IsCarAvailableAsync(dto.CarId, dto.StartDate, dto.EndDate);
        if (!isAvailable)
            throw new ConflictException("Car is not available for the selected dates.");

        if (car.Status != "Available")
            throw new ConflictException($"Car is currently {car.Status.ToLower()}. Cannot create rental.");

        var rental = new Rental
        {
            CustomerId = customerId,
            CarId = dto.CarId,
            BookingNumber = GenerateBookingNumber(),
            RentalDate = DateTime.UtcNow,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            Customer = customer,
            Car = car
        };

        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return MapToDto(rental, car);
    }

    public async Task<RentalResponseDto> GetRentalByIdAsync(int rentalId, int requesterId, bool isAdmin)
    {
        var query = _context.Rentals
            .AsNoTracking()
            .Include(r => r.Car)
            .Where(r => r.Id == rentalId);

        if (!isAdmin)
        {
            query = query.Where(r => r.CustomerId == requesterId);
        }

        var rental = await query.FirstOrDefaultAsync();

        if (rental == null)
            throw new NotFoundException("Rental", rentalId);

        return MapToDto(rental, rental.Car);
    }

    public async Task<PagedResult<RentalResponseDto>> GetCustomerRentalsAsync(int customerId, int page = 1, int pageSize = 10)
    {
        var customerExists = await _context.Customers.AsNoTracking().AnyAsync(c => c.Id == customerId);
        if (!customerExists)
            throw new NotFoundException("Customer", customerId);

        var (normalizedPage, normalizedPageSize) = NormalizePagination(page, pageSize);

        var query = _context.Rentals
            .AsNoTracking()
            .Where(r => r.CustomerId == customerId)
            .Include(r => r.Car)
            .OrderByDescending(r => r.CreatedAt);

        var totalCount = await query.CountAsync();
        var rentals = await query
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync();

        var items = rentals.Select(r => MapToDto(r, r.Car)).ToList();

        return new PagedResult<RentalResponseDto>
        {
            Items = items,
            Page = normalizedPage,
            PageSize = normalizedPageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PagedResult<RentalResponseDto>> GetAllRentalsAsync(int page = 1, int pageSize = 10)
    {
        var (normalizedPage, normalizedPageSize) = NormalizePagination(page, pageSize);

        var query = _context.Rentals
            .AsNoTracking()
            .Include(r => r.Car)
            .OrderByDescending(r => r.CreatedAt);

        var totalCount = await query.CountAsync();
        var rentals = await query
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync();

        var items = rentals.Select(r => MapToDto(r, r.Car)).ToList();

        return new PagedResult<RentalResponseDto>
        {
            Items = items,
            Page = normalizedPage,
            PageSize = normalizedPageSize,
            TotalCount = totalCount
        };
    }

    public async Task<RentalResponseDto> UpdateRentalStatusAsync(int rentalId, string status)
    {
        var dto = new UpdateRentalStatusDto { Status = status };
        await ValidateAsync(_updateStatusValidator, dto);

        var rental = await _context.Rentals
            .Include(r => r.Car)
            .FirstOrDefaultAsync(r => r.Id == rentalId);

        if (rental == null)
            throw new NotFoundException("Rental", rentalId);

        if (rental.Status == "Completed" || rental.Status == "Cancelled")
            throw new ConflictException($"Cannot update status of a {rental.Status.ToLower()} rental.");

        rental.Status = status;
        await _context.SaveChangesAsync();

        return MapToDto(rental, rental.Car);
    }

    public async Task CancelRentalAsync(int rentalId, int customerId)
    {
        var rental = await _context.Rentals
            .FirstOrDefaultAsync(r => r.Id == rentalId && r.CustomerId == customerId);

        if (rental == null)
            throw new NotFoundException("Rental", rentalId);

        if (rental.Status == "Completed" || rental.Status == "Cancelled")
            throw new ConflictException($"Cannot cancel a {rental.Status.ToLower()} rental.");

        if (rental.StartDate <= DateTime.UtcNow)
            throw new ConflictException("Cannot cancel a rental that has already started.");

        rental.Status = "Cancelled";
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate)
    {
        var hasConflict = await _context.Rentals
            .AsNoTracking()
            .Where(r => r.CarId == carId && r.Status != "Cancelled")
            .AnyAsync(r => startDate < r.EndDate && endDate > r.StartDate);

        return !hasConflict;
    }

    private string GenerateBookingNumber()
    {
        var suffix = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
        return $"BK{DateTime.UtcNow:yyyyMMddHHmmssfff}{suffix}";
    }

    private static (int Page, int PageSize) NormalizePagination(int page, int pageSize)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize < 1 ? 10 : pageSize;
        if (normalizedPageSize > 100) normalizedPageSize = 100;

        return (normalizedPage, normalizedPageSize);
    }

    private RentalResponseDto MapToDto(Rental rental, Car car)
    {
        var days = (rental.EndDate - rental.StartDate).Days;
        if (days == 0) days = 1;

        return new RentalResponseDto
        {
            Id = rental.Id,
            CustomerId = rental.CustomerId,
            CarId = rental.CarId,
            PaymentId = rental.PaymentId,
            BookingNumber = rental.BookingNumber,
            RentalDate = rental.RentalDate,
            StartDate = rental.StartDate,
            EndDate = rental.EndDate,
            Status = rental.Status,
            CarBrand = car.Brand,
            CarModel = car.Model,
            CarYear = car.Year,
            DailyPrice = car.Price,
            TotalPrice = car.Price * days,
            CreatedAt = rental.CreatedAt
        };
    }

    private async Task ValidateAsync<T>(IValidator<T> validator, T obj)
    {
        var validationResult = await validator.ValidateAsync(obj);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new AppValidationException(errors);
        }
    }
}
