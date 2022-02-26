using DaDiPortal.API.Models;
using Microsoft.AspNetCore.Components;


namespace DaDiPortal.Portal.Pages;
public partial class Donations
{
    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private IConfiguration Config { get; set; }

    public List<DonationModel> MyDonations { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var result = await HttpClient.GetAsync(Config["ApiUrl"] + "/api/donations");
        if (result.IsSuccessStatusCode)
            MyDonations = await result
                .Content
                .ReadFromJsonAsync<List<DonationModel>>();
    }
}
