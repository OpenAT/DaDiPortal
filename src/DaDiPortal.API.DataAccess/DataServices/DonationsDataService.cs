using DaDiPortal.API.DataAccess.DTOs;
using DaDiPortal.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DaDiPortal.API.DataAccess.DataServices;

public interface IDonationsDataService
{
    Task<IEnumerable<DonationDto>> GetDonations();
}

internal class DonationsDataService : IDonationsDataService
{
    private readonly ApiCtx _ctx;

    public DonationsDataService(ApiCtx ctx)
    {
        _ctx = ctx;
    }

    public async Task<IEnumerable<DonationDto>> GetDonations()
    {
        var dbDonations = await _ctx
            .Donations
            .ToArrayAsync();

        return dbDonations
            .Select(x => new DonationDto()
            {
                Date = x.Date,
                Amount = x.Amount,
            });
    }
}
