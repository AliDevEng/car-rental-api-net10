using FluentValidation;
using Microsoft.EntityFrameworkCore;
using CarRental.Application.DTOs.Customer;
using CarRental.Application.Exceptions;
using CarRental.Application.Services.Interfaces;
using CarRental.Core.Entities;
using CarRental.Infrastructure.Data;

namespace CarRental.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly CarRentalDbContext _context;
    private readonly IValidator<CreateCustomerDto> _createValidator;
    private readonly IValidator<UpdateCustomerDto> _updateValidator;

    public CustomerService(
        CarRentalDbContext context,
        IValidator<CreateCustomerDto> createValidator,
        IValidator<UpdateCustomerDto> updateValidator)
    {
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerDto dto)
    {
        await ValidateAsync(_createValidator, dto);

        // Check for duplicate email
        var emailExists = await _context.Customers.AnyAsync(c => c.Email == dto.Email);
        if (emailExists)
            throw new ConflictException($"A customer with email '{dto.Email}' already exists.");

        var customer = new Customer
        {
            Email = dto.Email,
            Password = dto.Password, // Plain text for now - BCrypt hashing in Phase 3
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            Address = dto.Address,
            City = dto.City,
            PostalCode = dto.PostalCode,
            Country = dto.Country,
            CreatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return MapToResponseDto(customer);
    }

    public async Task<CustomerResponseDto> GetCustomerByIdAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            throw new NotFoundException("Customer", id);

        return MapToResponseDto(customer);
    }

    public async Task<CustomerResponseDto> UpdateCustomerAsync(int id, UpdateCustomerDto dto)
    {
        await ValidateAsync(_updateValidator, dto);

        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            throw new NotFoundException("Customer", id);

        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.Phone = dto.Phone;
        customer.Address = dto.Address;
        customer.City = dto.City;
        customer.PostalCode = dto.PostalCode;
        customer.Country = dto.Country;

        await _context.SaveChangesAsync();

        return MapToResponseDto(customer);
    }

    private static CustomerResponseDto MapToResponseDto(Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            Email = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Phone = customer.Phone,
            Address = customer.Address,
            City = customer.City,
            PostalCode = customer.PostalCode,
            Country = customer.Country,
            CreatedAt = customer.CreatedAt
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
