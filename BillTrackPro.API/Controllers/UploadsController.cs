using BillTrackPro.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace BillTrackPro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadsController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public UploadsController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(ServiceResponse<string>.Fail("No file uploaded"));

        if (_environment.WebRootPath == null)
        {
            return BadRequest(ServiceResponse<string>.Fail("Storage for uploads is not available in the test environment."));
        }

        var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
        return Ok(ServiceResponse<string>.Ok(url, "File uploaded successfully"));
    }
}
