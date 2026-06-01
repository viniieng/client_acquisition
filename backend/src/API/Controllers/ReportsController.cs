using ClientAcquisition.Application.Common.Interfaces;
using ClientAcquisition.Application.Reports.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ClientAcquisition.Api.Controllers;

[ApiController]
[Route("api/reports")]
public sealed class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("customer-spending")]
    public async Task<ActionResult<IReadOnlyList<CustomerSpendingDto>>> GetCustomerSpending()
    {
        var result = await _reportService.GetCustomerSpendingAsync();
        return Ok(result);
    }

    [HttpGet("customer-spending/{customerId:guid}")]
    public async Task<ActionResult<CustomerSpendingDto>> GetCustomerSpendingByCustomerId(Guid customerId)
    {
        var result = await _reportService.GetCustomerSpendingAsync(customerId);
        return result is null ? NotFound() : Ok(result);
    }
}