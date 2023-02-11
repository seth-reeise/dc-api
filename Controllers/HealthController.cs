using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dc_api.Controllers;

public class HealthController : ControllerBase
{
    [HttpGet]
    [Route("health")]
    [AllowAnonymous]
    public IActionResult Get()
    {
        return Ok("Healthy!");
    }
}