using Microsoft.AspNetCore.Mvc;

namespace DaDiPortal.API.Controllers;

[ApiController, Route("api/[controller]")]
public class HomeController : Controller
{
    private readonly ILogger _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Received request for welcome message");
        return Ok("Welcome");
    }
}
