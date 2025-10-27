using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SecureController : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("sensitive")]
    public IActionResult GetSensitive() => Ok(new { secret = "only-for-admins" });

    [Authorize]
    [HttpGet("profile")]
    public IActionResult Profile() => Ok(new { user = User.Identity?.Name });
}
