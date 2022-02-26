using DaDiPortal.API.Models;
using DaDiPortal.Portal.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;


namespace DaDiPortal.Portal.Pages;
public partial class Donations
{
    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private IConfiguration Config { get; set; }
    [Inject] private ITokenService TokenService { get; set; }

    public List<DonationModel> MyDonations { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var tokenResponse = await TokenService
            .GetToken("DaDiPortalApi.read");

        if (!tokenResponse.IsError)
        {
            HttpClient.SetBearerToken(tokenResponse.AccessToken);

            var result = await HttpClient.GetAsync(Config["ApiUrl"] + "/api/donations");
            if (result.IsSuccessStatusCode)
                MyDonations = await result
                    .Content
                    .ReadFromJsonAsync<List<DonationModel>>();
        }
    }
}
