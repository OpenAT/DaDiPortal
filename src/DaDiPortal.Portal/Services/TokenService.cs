using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace DaDiPortal.Portal.Services;

public interface ITokenService
{
    Task<TokenResponse> GetToken(string scope);
}

public class TokenService : ITokenService
{
    private readonly IdentityServerSettings _settings;
    private readonly HttpClient _httpClient;

    public TokenService(IOptions<IdentityServerSettings> settings, HttpClient httpClient)
    {
        _settings = settings.Value;
        _httpClient = httpClient;
    }

    public async Task<TokenResponse> GetToken(string scope)
    {
        var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(_settings.DiscoveryUrl);
        if (!discoveryDocument.IsError)
        {
            var tokenRequest = new ClientCredentialsTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = _settings.ClientName,
                ClientSecret = _settings.ClientPassword
            };

            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(tokenRequest);

            if (tokenResponse.IsError)
                throw new Exception("Unable to get token", tokenResponse.Exception);

            return tokenResponse;
        }
        else
            throw new Exception("Unable to get discovery document", discoveryDocument.Exception);
    }
}
