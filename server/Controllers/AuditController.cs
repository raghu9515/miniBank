using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace miniBank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "logs", "audit.log");
        if (!System.IO.File.Exists(filePath))
        {
            return Ok(Array.Empty<string>());
        }

        var lines = System.IO.File.ReadAllLines(filePath).Reverse().Take(50).ToArray();
        return Ok(lines);
    }
}
