using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DaDiPortal.Portal.Shared;
public partial class RedirectToLogin
{
    [Inject] public NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        NavigationManager.NavigateTo($"/login?redirectUri={Uri.EscapeDataString(NavigationManager.Uri)}", true);
    }
}
