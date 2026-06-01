using ClientAcquisition.Application.Common.Interfaces;
using ClientAcquisition.Application.Customers.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ClientAcquisition.Api.Controllers;

[ApiController]
[Route("api/customers")]
public sealed class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CustomerResponseDto>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var customers = await _customerService.GetPagedAsync(page, pageSize);
        return Ok(customers);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponseDto>> GetById(Guid id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> Create([FromBody] CustomerCreateDto dto)
    {
        var created = await _customerService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CustomerResponseDto>> Update(Guid id, [FromBody] CustomerUpdateDto dto)
    {
        var updated = await _customerService.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _customerService.DeleteAsync(id);
        return NoContent();
    }
}