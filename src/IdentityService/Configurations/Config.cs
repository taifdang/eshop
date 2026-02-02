using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityService.Constants;

namespace IdentityService.Configurations;

public static class Config
{
    // Resource - ApiScope - ApiResource - Clients
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
    ];

    public static IEnumerable<ApiScope> ApiScopes => [
        new ApiScope(AuthScope.Read, "Read Access to API"),
        new ApiScope(AuthScope.Write, "Write Access to API"),
        new ApiScope(AuthScope.All, "Write and Read Access to API")
    ];

    public static IEnumerable<ApiResource> ApiResources => [
        new()
        {
            Name = "api.eshop",
            DisplayName = "Eshop API",
            Scopes = {AuthScope.Read, AuthScope.Write},
        }
    ];

    public static IEnumerable<Client> Clients => [

        // Resource Owner
        new()
        {
            ClientId = "ro.client",
            ClientName = "Resource Owner Client",
            ClientSecrets = {new Secret("secret".Sha256())},
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            AllowedScopes = { AuthScope.Read, AuthScope.Write},
        },
        // Backend For Frontend
        new()
        {
            ClientId = "bff",
            ClientName = "Backend For Frontend",
            ClientSecrets = {new Secret("secret".Sha256())},
            AllowedGrantTypes = [GrantType.AuthorizationCode],
            AllowedScopes = {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                AuthScope.Read,
                AuthScope.Write,
            },
            AllowOfflineAccess = true,
            AllowedCorsOrigins = { "https://localhost:5002"},
            AlwaysIncludeUserClaimsInIdToken = true,          
            RedirectUris = { "https://localhost:5002/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc"},
            FrontChannelLogoutUri = "https://localhost:5002/signout-oidc"
        },     
    ];
}
