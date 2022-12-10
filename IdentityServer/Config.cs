using IdentityServer4.Models;
using IdentityServer4;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiScope> ApiScopes =>
           new[] { new ApiScope("api1"), };


        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
        {
            new ApiResource("api1", "My API")
        };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
        {
            new Client
            {
                ClientId = "client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = { "api1" }
            },
            // resource owner password grant client
            new Client
            {
                ClientId = "ro.client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "api1" }
            },
            // OpenID Connect hybrid flow client (MVC)
            new Client
            {
                ClientId = "mvc",
                ClientName = "MVC Client",
                AllowedGrantTypes = GrantTypes.Code,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                RedirectUris           = { "https://localhost:7088/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:7088/signout-callback-oidc" },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1"
                },

                AllowOfflineAccess = true,
                RequirePkce = true,
            }
         };
        }
    }
}
