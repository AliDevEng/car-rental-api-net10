using Microsoft.AspNetCore.Mvc;
using CarRental.Application.DTOs.Car;
using CarRental.Application.DTOs.Common;
using CarRental.Application.Services.Interfaces;

namespace CarRental.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    /// <summary>
    /// Get all cars with optional filtering, sorting, and pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<CarResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<CarResponseDto>>> GetCars([FromQuery] CarFilterDto filter)
    {
        var result = await _carService.GetAllCarsAsync(filter);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific car by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CarResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarResponseDto>> GetCar(int id)
    {
        var car = await _carService.GetCarByIdAsync(id);
        return Ok(car);
    }

    /// <summary>
    /// Create a new car
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CarResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CarResponseDto>> CreateCar([FromBody] CreateCarDto dto)
    {
        var car = await _carService.CreateCarAsync(dto);
        return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
    }

    /// <summary>
    /// Update an existing car
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CarResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CarResponseDto>> UpdateCar(int id, [FromBody] UpdateCarDto dto)
    {
        var car = await _carService.UpdateCarAsync(id, dto);
        return Ok(car);
    }

    /// <summary>
    /// Delete a car
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteCar(int id)
    {
        await _carService.DeleteCarAsync(id);
        return NoContent();
    }
}
