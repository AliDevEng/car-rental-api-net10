using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CarRental.Application.DTOs.Auth;
using CarRental.Application.Exceptions;
using CarRental.Application.Services.Interfaces;
using CarRental.Core.Entities;
using CarRental.Infrastructure.Data;

namespace CarRental.Application.Services;

public class AuthService : IAuthService
{
    private readonly CarRentalDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<LoginDto> _loginValidator;

    public AuthService(
        CarRentalDbContext context,
        IConfiguration configuration,
        IValidator<RegisterDto> registerValidator,
        IValidator<LoginDto> loginValidator)
    {
        _context = context;
        _configuration = configuration;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        await ValidateAsync(_registerValidator, dto);

        // Check for duplicate email
        var emailExists = await _context.Customers.AnyAsync(c => c.Email == dto.Email);
        if (emailExists)
            throw new ConflictException($"A customer with email '{dto.Email}' already exists.");

        var customer = new Customer
        {
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
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

        var token = GenerateToken(customer.Id, customer.Email, "Customer");

        return new AuthResponseDto
        {
            Token = token.Token,
            ExpiresAt = token.ExpiresAt,
            Role = "Customer",
            UserId = customer.Id,
            Email = customer.Email
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        await ValidateAsync(_loginValidator, dto);

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == dto.Email);

        if (customer == null || !VerifyPassword(dto.Password, customer.Password))
            throw new UnauthorizedException("Invalid email or password.");

        var token = GenerateToken(customer.Id, customer.Email, "Customer");

        return new AuthResponseDto
        {
            Token = token.Token,
            ExpiresAt = token.ExpiresAt,
            Role = "Customer",
            UserId = customer.Id,
            Email = customer.Email
        };
    }

    public async Task<AuthResponseDto> AdminLoginAsync(LoginDto dto)
    {
        await ValidateAsync(_loginValidator, dto);

        var admin = await _context.Admins
            .FirstOrDefaultAsync(a => a.Email == dto.Email);

        if (admin == null || !VerifyPassword(dto.Password, admin.Password))
            throw new UnauthorizedException("Invalid email or password.");

        var token = GenerateToken(admin.Id, admin.Email, admin.Role);

        return new AuthResponseDto
        {
            Token = token.Token,
            ExpiresAt = token.ExpiresAt,
            Role = admin.Role,
            UserId = admin.Id,
            Email = admin.Email
        };
    }

    private (string Token, DateTime ExpiresAt) GenerateToken(int userId, string email, string role)
    {
        var jwtSecret = _configuration["JWT:Secret"]
            ?? throw new InvalidOperationException("JWT:Secret is not configured.");
        var expirationMinutes = int.Parse(_configuration["JWT:ExpirationMinutes"] ?? "60");
        var issuer = _configuration["JWT:Issuer"] ?? "CarRentalAPI";
        var audience = _configuration["JWT:Audience"] ?? "CarRentalClient";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
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

    private static bool VerifyPassword(string password, string storedHash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            // Legacy plain-text password from before Phase 3
            return false;
        }
    }
}
