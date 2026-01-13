using BillTrackPro.Application.Common;
using BillTrackPro.Domain.Entities;
using BillTrackPro.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BillTrackPro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly IRepository<UserProfile> _repository;

    public SettingsController(IRepository<UserProfile> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // Assuming single user with ID 1
        var profile = await _repository.GetByIdAsync(1);
        if (profile == null) 
            return NotFound(ServiceResponse<UserProfile>.Fail("Settings not found"));
            
        return Ok(ServiceResponse<UserProfile>.Ok(profile));
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserProfile profile)
    {
        var existing = await _repository.GetByIdAsync(1);
        if (existing == null)
            return NotFound(ServiceResponse<bool>.Fail("Settings not found"));

        // Manual mapping or just properties update
        existing.FullName = profile.FullName;
        existing.Title = profile.Title;
        existing.Email = profile.Email;
        existing.Phone = profile.Phone;
        existing.Timezone = profile.Timezone;
        existing.AvatarUrl = profile.AvatarUrl;
        
        existing.CompanyName = profile.CompanyName;
        existing.TaxId = profile.TaxId;
        existing.StreetAddress = profile.StreetAddress;
        existing.City = profile.City;
        existing.ZipCode = profile.ZipCode;
        existing.Country = profile.Country;
        existing.CompanyLogoUrl = profile.CompanyLogoUrl;
        existing.BrandColor = profile.BrandColor;
        existing.SecondaryColor = profile.SecondaryColor;

        await _repository.UpdateAsync(existing);
        
        return Ok(ServiceResponse<UserProfile>.Ok(existing, "Settings updated successfully"));
    }
}
