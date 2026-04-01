using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CarRental.Application.DTOs.Rental;
using CarRental.Application.Services.Interfaces;

namespace CarRental.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalsController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    /// <summary>
    /// Create a new rental booking (requires authentication)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(RentalResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RentalResponseDto>> CreateRental([FromBody] CreateRentalDto dto)
    {
        var customerId = GetUserId();
        var result = await _rentalService.CreateRentalAsync(customerId, dto);
        return CreatedAtAction(nameof(GetRental), new { id = result.Id }, result);
    }

    /// <summary>
    /// Get rental details by ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Customer,Admin")]
    [ProducesResponseType(typeof(RentalResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RentalResponseDto>> GetRental(int id)
    {
        var userId = GetUserId();
        var isAdmin = User.IsInRole("Admin");
        var result = await _rentalService.GetRentalByIdAsync(id, userId, isAdmin);
        return Ok(result);
    }

    /// <summary>
    /// Get all rentals for the current customer
    /// </summary>
    [HttpGet("my-rentals")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> GetMyRentals([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var customerId = GetUserId();
        var result = await _rentalService.GetCustomerRentalsAsync(customerId, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Get all rentals (admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> GetAllRentals([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _rentalService.GetAllRentalsAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Update rental status (admin only)
    /// </summary>
    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RentalResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RentalResponseDto>> UpdateRentalStatus(int id, [FromBody] UpdateRentalStatusDto dto)
    {
        var result = await _rentalService.UpdateRentalStatusAsync(id, dto.Status);
        return Ok(result);
    }

    /// <summary>
    /// Cancel a rental booking
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Customer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> CancelRental(int id)
    {
        var customerId = GetUserId();
        await _rentalService.CancelRentalAsync(id, customerId);
        return NoContent();
    }

    /// <summary>
    /// Check if a car is available for dates
    /// </summary>
    [HttpGet("availability")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CheckAvailability(
        [FromQuery] int carId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        if (carId <= 0)
            return BadRequest(new { message = "Car ID must be greater than 0." });

        if (startDate >= endDate)
            return BadRequest(new { message = "Start date must be before end date." });

        var isAvailable = await _rentalService.IsCarAvailableAsync(carId, startDate, endDate);
        return Ok(new { carId, startDate, endDate, isAvailable });
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst(ClaimTypes.Name) ?? User.FindFirst("sub");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            throw new UnauthorizedAccessException("User ID not found in token.");

        return userId;
    }
}
