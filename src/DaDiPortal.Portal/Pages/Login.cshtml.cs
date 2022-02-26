using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DaDiPortal.Portal.Pages;

public class LoginModel : PageModel
{
    public async Task<IActionResult> OnGetAsync(string redirectUri)
    {
        if (string.IsNullOrEmpty(redirectUri))
            redirectUri = Url.Content("~/");

        if (HttpContext?.User?.Identity?.IsAuthenticated == true)
            Response.Redirect(redirectUri);

        var authProps = new AuthenticationProperties
        {
            RedirectUri = redirectUri
        };

        return Challenge(authProps, OpenIdConnectDefaults.AuthenticationScheme);
    }
}
