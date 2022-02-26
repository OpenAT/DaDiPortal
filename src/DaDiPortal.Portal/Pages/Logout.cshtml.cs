using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DaDiPortal.Portal.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly IConfiguration _config;

        public LogoutModel(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var authProps = new AuthenticationProperties() 
            { 
                RedirectUri = _config["ApplicationUrl"] 
            };

            return SignOut(
                authProps, 
                OpenIdConnectDefaults.AuthenticationScheme, 
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
