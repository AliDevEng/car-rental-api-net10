using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarRental.Application.DTOs.Admin;
using CarRental.Application.DTOs.Customer;
using CarRental.Application.Services.Interfaces;

namespace CarRental.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    /// <summary>
    /// Get dashboard statistics
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(DashboardStatsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardStatsDto>> GetStats()
    {
        var stats = await _adminService.GetDashboardStatsAsync();
        return Ok(stats);
    }

    /// <summary>
    /// Get all customers
    /// </summary>
    [HttpGet("customers")]
    [ProducesResponseType(typeof(List<CustomerResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CustomerResponseDto>>> GetAllCustomers()
    {
        var customers = await _adminService.GetAllCustomersAsync();
        return Ok(customers);
    }
}
