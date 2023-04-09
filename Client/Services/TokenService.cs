using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Client.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<IdentitySettings> _identitySettings;
        private readonly DiscoveryDocumentResponse _documentResponse;
        private readonly HttpClient _httpClient;

        public TokenService(IOptions<IdentitySettings>  identitySettings)
        {
            _identitySettings = identitySettings;
            _httpClient = new HttpClient();
            _documentResponse = _httpClient.GetDiscoveryDocumentAsync
                 (_identitySettings.Value.DiscoveryUrl).Result;

            if (_documentResponse.IsError)
            {
                throw new Exception("Unable to get discovery document", _documentResponse.Exception);
            }
           
        }

        public async Task<TokenResponse> GetToken(string scope)
        {
            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest { 
                Address = _documentResponse.TokenEndpoint,
                ClientId = _identitySettings.Value.ClientName,
                ClientSecret = _identitySettings.Value.ClientPassword,
                Scope = scope
            });

            if (tokenResponse.IsError)
            {
                throw new Exception("Unable to get token", tokenResponse.Exception);
            }

            return tokenResponse;
        }
    }
}
