using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DaDiPortal.Portal.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(ILogger<LoginModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGetAsync(string redirectUri)
    {
        if (string.IsNullOrEmpty(redirectUri))
            redirectUri = Url.Content("~/");

        if (HttpContext?.User?.Identity?.IsAuthenticated == true)
            Response.Redirect(redirectUri);

        var authProps = new AuthenticationProperties
        {
            RedirectUri = redirectUri
        };

        _logger.LogInformation($"Logging in. Redirecting to Challange with url {redirectUri}");
        return Challenge(authProps, OpenIdConnectDefaults.AuthenticationScheme);
    }
}

public interface ILogWrapper
{
    void LogInformation(string info);
}

public class LogWrapper : ILogWrapper
{
    private readonly ILogger<LoginModel> _logger;

    public LogWrapper(ILogger<LoginModel> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string info)
    {
        _logger.LogInformation(info);
    }
}

