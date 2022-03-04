using Microsoft.AspNetCore.Mvc;

namespace DaDiPortal.API.Controllers;

[ApiController, Route("api/[controller]")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Welcome");
    }
}
