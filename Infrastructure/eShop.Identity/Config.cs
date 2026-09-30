using Duende.IdentityServer.Models;

namespace eShop.Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("catalogapi"),
            new ApiScope("basketapi"),
            new ApiScope("catalogapi.read"),
            new ApiScope("catalogapi.write"),
            new ApiScope("eshoppinggateway"),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
         new ApiResource("Catalog", "Catalog API")
         {
             Scopes = { "catalogapi.write" , "catalogapi.read" }
         },
         new ApiResource("Basket", "Basket API")
         {
             Scopes = { "basketapi" }
         },
         new ApiResource("EShoppingGateway", "eshoppinggateway API")
         {
             Scopes = { "eshoppinggateway" }
         }
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            
            new Client
            {
                ClientId = "catalogapi",
                ClientName = "Catalog API",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("49C1A8V0-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                AllowedScopes = { "catalogapi.write", "catalogapi.read" }
            },
             new Client
            {
                ClientId = "basketapi",
                ClientName = "Basket API",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("49C1A8A2-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                AllowedScopes = {  "basketapi" }
            },
             new Client
            {
                ClientId = "eshoppinggateway",
                ClientName = "eshoppinggateway API",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("49C1A8A2-0C79-4A70-A3D6-A37998FB86B0".Sha256()) },
                AllowedScopes = { "eshoppinggateway", "basketapi" }
            }
        };
}
