using BillTrackPro.Application.Common;
using BillTrackPro.Application.DTOs;
using BillTrackPro.Application.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BillTrackPro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;
    private readonly IValidator<CreateInvoiceDto> _validator;

    public InvoicesController(IInvoiceService invoiceService, IValidator<CreateInvoiceDto> validator)
    {
        _invoiceService = invoiceService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, [FromQuery] string? status = null, [FromQuery] string? orderBy = null, [FromQuery] bool isDescending = false)
    {
        var response = await _invoiceService.GetPagedInvoicesAsync(page, pageSize, search, status, orderBy, isDescending);
        return Ok(response.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _invoiceService.GetInvoiceByIdAsync(id);
        if (!response.Success) return NotFound(response.Message);
        return Ok(response.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateInvoiceDto dto)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(ServiceResponse<InvoiceDto>.Fail("Validation failed", 
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
        }

        var response = await _invoiceService.CreateInvoiceAsync(dto);
        if (!response.Success) return BadRequest(response.Message);
        
        return CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateInvoiceDto dto)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
             return BadRequest(ServiceResponse<bool>.Fail("Validation failed", 
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()));

        var response = await _invoiceService.UpdateInvoiceAsync(id, dto);
        if (!response.Success) return NotFound(response.Message);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _invoiceService.DeleteInvoiceAsync(id);
        if (!response.Success) return NotFound(response.Message);
        return NoContent();
    }

    [HttpGet("dashboard-stats")]
    public async Task<IActionResult> GetStats()
    {
        var response = await _invoiceService.GetDashboardStatsAsync();
        return Ok(response.Data);
    }

    [HttpGet("reports")]
    public async Task<IActionResult> GetReports()
    {
        var response = await _invoiceService.GetReportsDataAsync();
        return Ok(response.Data);
    }
}
