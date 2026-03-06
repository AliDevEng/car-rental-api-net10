using CarRental.Application.DTOs.Auth;

namespace CarRental.Application.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> AdminLoginAsync(LoginDto dto);
}
