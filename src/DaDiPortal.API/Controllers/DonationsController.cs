using DaDiPortal.API.DataAccess.DataServices;
using DaDiPortal.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaDiPortal.API.Controllers;

[ApiController, Route("api/[controller]"), Authorize]
public class DonationsController : ControllerBase
{
    private readonly IDonationsDataService _dataService;

    public DonationsController(IDonationsDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var donations = await _dataService.GetDonations();
        return Ok(donations
            .Select(x => new DonationModel()
            {
                Date = x.Date,
                Amount = x.Amount
            })
            .ToArray());

    }
}
