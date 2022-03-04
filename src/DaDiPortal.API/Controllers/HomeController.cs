using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace DaDiPortal.API.Controllers;

[ApiController, Route("api/[controller]")]
public class HomeController : Controller
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IConfiguration config)
    {
        _logger = logger;
        _configuration = config;    
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Received request for welcome message");

        return Ok(new StringBuilder()
            .AppendLine($"Welcome!\n")
            .AppendLine($"Using Config:")
            .AppendLine($"\tAuthority: {_configuration["IdentityServerSettings:Authority"]}")
            .AppendLine($"\tApiName: {_configuration["IdentityServerSettings:ApiName"]}")
            .ToString());
    }
}
